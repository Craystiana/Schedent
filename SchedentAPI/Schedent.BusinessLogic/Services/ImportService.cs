using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using Schedent.BusinessLogic.Config;
using Schedent.BusinessLogic.Factories;
using Schedent.Common.Enums;
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

        public ImportService(IUnitOfWork unitOfWork, IOptions<GoogleCalendarSettings> googleCalendarSettings) : base(unitOfWork) 
        { 
            _googleCalendarSettings = googleCalendarSettings.Value;
        }

        public IEnumerable<Document> GetDocumentsToImport()
        {
            return UnitOfWork.DocumentRepository.Find(d => !d.DocumentTimeTables.Any())
                                                .OrderBy(d => d.CreatedOn)
                                                .ToList();
        }

        public void ImportTimeTableAsync()
        {
            var documents = GetDocumentsToImport();

            foreach (var document in documents)
            {
                try
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
                            var group = UnitOfWork.GroupRepository.SingleOrDefault(g => g.Name == cells[row, 1].Value.ToString() && g.SectionId == section.SectionId);
                            var subgroup = UnitOfWork.SubgroupRepository.SingleOrDefault(s => s.Name == cells[row, 2].Value.ToString() && s.GroupId == group.GroupId);
                            var day = cells[row, 3].Value.ToString();
                            var start = cells[row, 4].Value.ToString();
                            int.TryParse(cells[row, 5].Value.ToString(), out var duration);
                            var subject = UnitOfWork.SubjectRepository.SingleOrDefault(s => s.Name == cells[row, 6].Value.ToString() && s.SectionId == section.SectionId);
                            var type = UnitOfWork.ScheduleTypeRepository.SingleOrDefault(st => st.Name == cells[row, 7].Value.ToString());
                            var professor = UnitOfWork.ProfessorRepository.SingleOrDefault(p => p.Name == cells[row, 8].Value.ToString() && p.FacultyId == faculty.FacultyId);
                            int.TryParse(cells[row, 9].Value.ToString(), out var week);

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

                            continueProcessing = cells[++row, 1].Value != null;
                        }
                    }

                    ImportTimeTable(importLines, document);
                } 
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }

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
                });

                UnitOfWork.ScheduleRepository.AddRange(schedules);

                var notifications = AddNotifications(timeTable);
                AddEvents(timeTable);
                InactivateSubgroupTimeTable(timeTable.SubgroupId);
            }

            Save();
        }

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

        public void InactivateSubgroupTimeTable(int subgroupId)
        {
            var timetable = UnitOfWork.TimeTableRepository.SingleOrDefault(t => t.IsActive && t.SubgroupId == subgroupId);
            if (timetable != null)
            {
                timetable.IsActive = false;
            }
        }

        public IEnumerable<Domain.Entities.Notification> AddNotifications(TimeTable newTimeTable)
        {
            var oldTimeTable = UnitOfWork.TimeTableRepository.SingleOrDefault(t => t.IsActive && t.SubgroupId == newTimeTable.SubgroupId);
            var notifications = new List<Domain.Entities.Notification>();

            if (oldTimeTable == null)
            {
                notifications.Add(new Domain.Entities.Notification
                {
                    Message = "Your first timetable has been uploaded! Please check your schedule",
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

                    if (newSchedule.ProfessorId != oldSchedule.ProfessorId)
                    {
                        notifications.Add(notificationFactory.GetNotification(nameof(newSchedule.ProfessorId), oldSchedule, newSchedule));
                    }

                    if (newSchedule.Day != oldSchedule.Day)
                    {
                       notifications.Add(notificationFactory.GetNotification(nameof(newSchedule.Day), oldSchedule, newSchedule));
                    }

                    if (newSchedule.Duration != oldSchedule.Duration)
                    {
                       notifications.Add(notificationFactory.GetNotification(nameof(newSchedule.Duration), oldSchedule, newSchedule));
                    }

                    if (newSchedule.StartsAt != oldSchedule.StartsAt)
                    {
                       notifications.Add(notificationFactory.GetNotification(nameof(newSchedule.StartsAt), oldSchedule, newSchedule));
                    }

                    if (newSchedule.Week != oldSchedule.Week)
                    {
                       notifications.Add(notificationFactory.GetNotification(nameof(newSchedule.Week), oldSchedule, newSchedule));
                    }
                }
            }

            UnitOfWork.NotificationRepository.AddRange(notifications);

            return notifications;
        }

        public void AddEvents(TimeTable timeTable)
        {
            foreach (var schedule in timeTable.Schedules)
            {
                CreateEvent(schedule);
            }
        }

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

        public void CreateEvent(Schedule schedule)
        {
            var semDayStart = "21";
            var semMonthStart = "02";
            var yearStart = "2022";
            var atendeeEmails = UnitOfWork.UserRepository.Find(u => u.SubgroupId == schedule.TimeTable.SubgroupId)
                                                         .Select(u => new EventAttendee
                                                         {
                                                             Email = u.Email
                                                         }).ToList();

            var ev = new Event
            {
                Summary = schedule.Subject.Name,
                Start = new EventDateTime
                {
                    DateTime = Convert.ToDateTime((int.Parse(semDayStart) + (int)Enum.Parse(typeof(WeekDays), schedule.Day)).ToString() + "/" + semMonthStart + "/" + yearStart + " " + schedule.StartsAt + ":00:00"),
                    TimeZone = "Europe/Bucharest"
                },
                End = new EventDateTime
                {
                    DateTime = Convert.ToDateTime((int.Parse(semDayStart) + (int)Enum.Parse(typeof(WeekDays), schedule.Day)).ToString() + "/" + semMonthStart + "/" + yearStart + " " + (int.Parse(schedule.StartsAt) + schedule.Duration).ToString() + ":00:00"),
                    TimeZone = "Europe/Bucharest"
                },
                Recurrence = new String[]
                {
                    "RRULE:FREQ=WEEKLY;UNTIL=20220508T200000Z"
                },
                Attendees = new List<EventAttendee>() { new EventAttendee
                {
                    Email = "cristinamadalinaprodan@gmail.com",
                }, 
                new EventAttendee
                {
                    Email = "prodan.cristiana.c3s@student.ucv.ro",
                }}
            };

            GetService().Events.Insert(ev, "primary").Execute();
        }

        public async Task SendFirebaseNotification(IEnumerable<Domain.Entities.Notification> notifications)
        {
            foreach (var notification in notifications)
            {
                var message = new MulticastMessage
                {
                    Tokens = notification.Subgroup.Users.Select(u => u.DeviceToken).ToList(),
                    Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = "Orarul tau a fost modificat",
                        Body = notification.Message
                    },
                };

                var result = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
            }
        }
    }
}
