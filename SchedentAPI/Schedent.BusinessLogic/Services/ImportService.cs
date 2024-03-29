﻿using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using Schedent.BusinessLogic.Config;
using Schedent.BusinessLogic.Factories;
using Schedent.BusinessLogic.Helpers;
using Schedent.Domain.DTO.Import;
using Schedent.Domain.Entities;
using Schedent.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Schedent.BusinessLogic.Services
{
    public class ImportService : BaseService
    {
        private readonly GoogleCalendarSettings _googleCalendarSettings;

        /// <summary>
        /// ImportService constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="googleCalendarSettings"></param>
        public ImportService(IUnitOfWork unitOfWork, IOptions<GoogleCalendarSettings> googleCalendarSettings) : base(unitOfWork) 
        { 
            _googleCalendarSettings = googleCalendarSettings.Value;
        }

        /// <summary>
        /// Retrieve the unimported documents
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Document> GetDocumentsToImport()
        {
            return UnitOfWork.DocumentRepository.Find(d => !d.DocumentTimeTables.Any())
                                                .OrderBy(d => d.CreatedOn)
                                                .ToList();
        }

        /// <summary>
        /// Retrieve the Users without events
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetUsersWithoutSentEvents()
        {
            return UnitOfWork.UserRepository.Find(u => !u.EventsSent).ToList();
        }

        /// <summary>
        /// Import a document timetable
        /// </summary>
        /// <returns></returns>
        public async Task ImportTimeTableAsync()
        {
            var documents = GetDocumentsToImport();
            var users = GetUsersWithoutSentEvents();

            foreach (var document in documents)
            {
                var importLines = new List<ImportLine>();

                using (var package = new ExcelPackage(new MemoryStream(document.File)))
                {
                    var cells = package.Workbook.Worksheets.First().Cells;
                    var continueProcessing = true;
                    var faculty = UnitOfWork.FacultyRepository.SingleOrDefault(f => f.Name == cells[1, 1].Value.ToString());
                    var section = UnitOfWork.SectionRepository.SingleOrDefault(s => s.Name == cells[2, 1].Value.ToString() && s.FacultyId == faculty.FacultyId);
                    var row = 4;

                    while (continueProcessing)
                    {
                        AddRow(importLines, cells, faculty, section, row);

                        continueProcessing = cells[++row, 1].Value != null;
                    }
                }

                ImportTimeTable(importLines, document);
                var notifications = UnitOfWork.NotificationRepository.GetNotificationsByIds();
                await ImportHelper.SendFirebaseNotificationsAsync(notifications);
            }

            foreach (var user in users)
            {
                var schedules = user.ProfessorId != null ?
                            UnitOfWork.ScheduleRepository.GetSchedulesForProfessor(user.UserId).ToList() :
                            UnitOfWork.ScheduleRepository.GetSchedulesForStudent(user.UserId).ToList();

                foreach (var schedule in schedules)
                {
                    if (schedule.EventId != null)
                    {
                        AddAtendeeToEvent(schedule);
                    }
                    else
                    {
                        CreateEvent(schedule, false);
                    }
                }

                user.EventsSent = true;
                Save();
            }
        }

        /// <summary>
        /// Add a new imported schedule
        /// </summary>
        /// <param name="importLines"></param>
        /// <param name="cells"></param>
        /// <param name="faculty"></param>
        /// <param name="section"></param>
        /// <param name="row"></param>
        private void AddRow(List<ImportLine> importLines, ExcelRange cells, Faculty faculty, Section section, int row)
        {
            var group = UnitOfWork.GroupRepository.SingleOrDefault(g => g.Name == cells[row, 1].Value.ToString() && g.SectionId == section.SectionId);
            var subgroup = UnitOfWork.SubgroupRepository.SingleOrDefault(s => s.Name == cells[row, 2].Value.ToString() && s.GroupId == group.GroupId);
            var day = cells[row, 3].Value.ToString();
            var start = cells[row, 4].Value.ToString();
            _ = int.TryParse(cells[row, 5].Value.ToString(), out var duration);
            var subject = UnitOfWork.SubjectRepository.SingleOrDefault(s => s.Name == cells[row, 6].Value.ToString() && s.SectionId == section.SectionId);
            var type = UnitOfWork.ScheduleTypeRepository.SingleOrDefault(st => st.Name == cells[row, 7].Value.ToString());
            var professor = UnitOfWork.ProfessorRepository.SingleOrDefault(p => p.Name == cells[row, 8].Value.ToString() && p.FacultyId == faculty.FacultyId);
            _ = int.TryParse(cells[row, 9].Value.ToString(), out var week);

            importLines.Add(new ImportLine
            {
                Group = group,
                Subgroup = subgroup,
                Day = day,
                Start = start,
                Duration = duration,
                Subject = subject,
                Type = type,
                Professor = professor,
                Week = week
            });
        }

        /// <summary>
        /// Add the imported schedules to the database
        /// </summary>
        /// <param name="importLines"></param>
        /// <param name="document"></param>
        public void ImportTimeTable(IEnumerable<ImportLine> importLines, Document document)
        {
            var groupedSchedules = importLines.GroupBy(il => il.Subgroup);

            foreach (var subgroupSchedules in groupedSchedules)
            {
                var timeTable = AddTimeTable(subgroupSchedules.Key, document);
                var schedules = subgroupSchedules.Select(ss => new Schedule
                {
                    ScheduleTypeId = ss.Type.ScheduleTypeId,
                    SubjectId = ss.Subject.SubjectId,
                    ProfessorId = ss.Professor.ProfessorId,
                    TimeTable = timeTable,
                    Week = ss.Week,
                    Day = ss.Day,
                    StartsAt = ss.Start,
                    Duration = ss.Duration,
                    CreatedOn = DateTime.Now
                }).ToList();

                UnitOfWork.ScheduleRepository.AddRange(schedules);

                AddNotifications(timeTable);
                AddEvents(timeTable);
                InactivateSubgroupTimeTable(timeTable.SubgroupId);
            }

            Save();
        }

        /// <summary>
        /// Add a new timetable entity
        /// </summary>
        /// <param name="subgroup"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        public TimeTable AddTimeTable(Subgroup subgroup, Document document)
        {
            var timeTable = new TimeTable
            {
                SubgroupId = subgroup.SubgroupId,
                IsActive = true,
                DocumentTimeTables = new List<DocumentTimeTable>
                {
                    new DocumentTimeTable
                    {
                        DocumentId = document.DocumentId
                    }
                }
            };

            UnitOfWork.TimeTableRepository.Add(timeTable);

            return timeTable;
        }

        /// <summary>
        /// Retrieve the current timetable of the given subgroup
        /// </summary>
        /// <param name="subgroupId"></param>
        /// <returns></returns>
        private TimeTable GetCurrentTimeTableForSubgroup(int subgroupId)
        {
            return UnitOfWork.TimeTableRepository.SingleOrDefault(t => t.IsActive && t.SubgroupId == subgroupId);
        }

        /// <summary>
        /// Inactive the old timetable
        /// </summary>
        /// <param name="subgroupId"></param>
        public void InactivateSubgroupTimeTable(int subgroupId)
        {
            TimeTable timetable = GetCurrentTimeTableForSubgroup(subgroupId);
            if (timetable != null)
            {
                timetable.IsActive = false;
            }
        }

        /// <summary>
        /// Add notifications with the changes from the imported timetable
        /// </summary>
        /// <param name="newTimeTable"></param>
        /// <returns></returns>
        public IEnumerable<Domain.Entities.Notification> AddNotifications(TimeTable newTimeTable)
        {
            var oldTimeTable = GetCurrentTimeTableForSubgroup(newTimeTable.SubgroupId);
            var notifications = new List<Domain.Entities.Notification>();

            if (oldTimeTable == null)
            {
                notifications.Add(new Domain.Entities.Notification
                {
                    Message = "Primul orar a fost încărcat! Acum poți să verifici activitățile didactice.",
                    SubgroupId = newTimeTable.SubgroupId,
                    IsSent = false,
                    CreatedOn = DateTime.Now,
                });
            }
            else 
            { 
                var oldSchedules = UnitOfWork.ScheduleRepository.GetSchedulesForTimeTable(oldTimeTable.TimeTableId);
                var newSchedules = newTimeTable.Schedules;
                var notificationFactory = new NotificationFactory();

                foreach (var newSchedule in newSchedules)
                {
                    var oldSchedule = oldSchedules.FirstOrDefault(os => os.SubjectId == newSchedule.SubjectId
                                                                     && os.ScheduleTypeId == newSchedule.ScheduleTypeId);

                    if (oldSchedule == null) continue;
                    ImportHelper.AddNotifications(notifications, notificationFactory, newSchedule, oldSchedule);
                }
            }

            UnitOfWork.NotificationRepository.AddRange(notifications);

            return notifications;
        }

        /// <summary>
        /// Add Google Calendar events
        /// </summary>
        /// <param name="timeTable"></param>
        public void AddEvents(TimeTable timeTable)
        {
            foreach (var schedule in timeTable.Schedules)
            {
                CreateEvent(schedule);
            }
        }

        /// <summary>
        /// Retrieve the Google Calendar service
        /// </summary>
        /// <returns></returns>
        public CalendarService GetService()
        {
            string[] Scopes = { CalendarService.Scope.Calendar };

            var clientSecret = new ClientSecrets
            {
                ClientId = _googleCalendarSettings.ClientId,
                ClientSecret = _googleCalendarSettings.ClientSecret
            };

            string credPath = "token.json";

            return new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = GoogleWebAuthorizationBroker.AuthorizeAsync(clientSecret, Scopes, "user", CancellationToken.None, new FileDataStore(credPath, true)).Result
            });
        }

        /// <summary>
        /// Create a new Google Calendar event
        /// </summary>
        /// <param name="schedule"></param>
        public void CreateEvent(Schedule schedule, bool isUpdatedEvent = true)
        {
            var semDayStart = schedule.Week == 2 ? "21" : "14";
            var semMonthStart = "02";
            var yearStart = "2022";
            var recurrence = $"{"RRULE:FREQ=WEEKLY;UNTIL=20220508T200000Z"}{(schedule.Week == 0 ? "" : ";INTERVAL=2")}";
            List<EventAttendee> atendeeEmails = GetAtendeeEmails(schedule);
            atendeeEmails.AddRange(UnitOfWork.UserRepository.Find(u => u.ProfessorId == schedule.ProfessorId).Select(u => new EventAttendee
            {
                Email = u.Email
            }).ToList());
            var service = GetService();

            if (atendeeEmails.Any())
            {
                var ev = ImportHelper.CreateEventEntity(schedule, semDayStart, semMonthStart, yearStart, recurrence, atendeeEmails);

                var createdEvent = service.Events.Insert(ev, "primary").Execute();
                schedule.EventId = createdEvent.Id;
            }

            if (isUpdatedEvent)
            {
                var oldSchedule = UnitOfWork.ScheduleRepository.SingleOrDefault(s => s.TimeTable.SubgroupId == schedule.TimeTable.SubgroupId && s.TimeTable.IsActive && s.SubjectId == schedule.SubjectId && s.ScheduleType == schedule.ScheduleType);
                if (oldSchedule is not null && oldSchedule.EventId is not null)
                {
                    try { service.Events.Delete("primary", oldSchedule.EventId).Execute(); }
                    catch { /* If the Google Calendar deletion throws an error it means that the event is already deleted, therefore nothing should be done */ }
                }
            }
        }

        /// <summary>
        /// Add user to already existing event
        /// </summary>
        /// <param name="schedule"></param>
        public void AddAtendeeToEvent(Schedule schedule)
        {
            var service = GetService();
            var atendeeEmails = GetAtendeeEmails(schedule);
            atendeeEmails.AddRange(UnitOfWork.UserRepository.Find(u => u.ProfessorId == schedule.ProfessorId).Select(u => new EventAttendee
            {
                Email = u.Email
            }).ToList());

            var existingEvent = service.Events.Get("primary", schedule.EventId).Execute();
            if (existingEvent != null && existingEvent.Status != "cancelled")
            {
                existingEvent.Attendees = atendeeEmails;
                service.Events.Patch(existingEvent, "primary", schedule.EventId).Execute();
            }
            else
            {
                CreateEvent(schedule, false);
            }
        }

        /// <summary>
        /// Get the list of emails for event atendees
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        private List<EventAttendee> GetAtendeeEmails(Schedule schedule)
        {
            return UnitOfWork.UserRepository.Find(u => u.SubgroupId == schedule.TimeTable.SubgroupId)
                                            .Select(u => new EventAttendee
                                            {
                                                Email = u.Email
                                            }).ToList();
        }
    }
}
