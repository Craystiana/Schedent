using OfficeOpenXml;
using Schedent.BusinessLogic.Factories;
using Schedent.Domain.DTO.Import;
using Schedent.Domain.Entities;
using Schedent.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Schedent.BusinessLogic.Services
{
    public class ImportService : BaseService
    {
        public ImportService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

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

                AddNotifications(timeTable);
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
            timetable.IsActive = false;
        }

        public void AddNotifications(TimeTable newTimeTable)
        {
            var oldTimeTable = UnitOfWork.TimeTableRepository.SingleOrDefault(t => t.IsActive && t.SubgroupId == newTimeTable.SubgroupId);
            var notifications = new List<Notification>();

            if (oldTimeTable == null)
            {
                notifications.Add(new Notification
                {
                    Message = "Your first timetable has been uploaded! Please check your schedule",
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
        }
    }
}
