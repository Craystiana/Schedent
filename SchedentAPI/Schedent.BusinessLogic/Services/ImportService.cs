using OfficeOpenXml;
using Schedent.Domain.Entities;
using Schedent.Domain.Interfaces;
using System;
using System.IO;
using System.Linq;

namespace Schedent.BusinessLogic.Services
{
    public class ImportService : BaseService
    {
        public ImportService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public Document GetLastDocument()
        {
            return UnitOfWork.DocumentRepository.GetAll().OrderByDescending(d => d.CreatedOn).FirstOrDefault();
        }

        public Document GetDocumentToImport()
        {
            var document = GetLastDocument();

            var documentTimeTable = UnitOfWork.DocumentTimeTableRepository.Find(dt => dt.DocumentId == document.DocumentId).FirstOrDefault();

            return documentTimeTable != null ? null : document;
        }

        public string ConvertToBase64String(byte[] byteArray)
        {
            return byteArray == null ? null : Convert.ToBase64String(byteArray);
        }

        public int ConvertObjectToInt(object obj)
        {
            int.TryParse(obj.ToString(), out var result);

            return result;
        }

        public void ImportTimeTable()
        {
            var document = GetDocumentToImport();

            if (document != null)
            {
                try
                {
                    using (var package = new ExcelPackage(new MemoryStream(document.File)))
                    {
                        var cells = package.Workbook.Worksheets.First().Cells;
                        var hasValue = true;
                        var faculty = UnitOfWork.FacultyRepository.Find(f => f.Name == cells[1, 1].Value.ToString()).FirstOrDefault();
                        var section = UnitOfWork.SectionRepository.Find(s => s.Name == cells[2, 1].Value.ToString() && s.FacultyId == faculty.FacultyId).FirstOrDefault();
                        var lastSubgroup = new Subgroup();
                        var timeTable = new TimeTable();
                        var row = 4;
                        do
                        {
                            var col = 1;
                            var group = UnitOfWork.GroupRepository.Find(g => g.Name == cells[row, 1].Value.ToString() && g.SectionId == section.SectionId).FirstOrDefault();
                            var subgroup = UnitOfWork.SubgroupRepository.Find(s => s.Name == cells[row, 2].Value.ToString() && s.GroupId == group.GroupId).FirstOrDefault();
                            var day = cells[row, 3].Value.ToString();
                            var start = cells[row, 4].Value.ToString();
                            var duration = ConvertObjectToInt(cells[row, 5].Value);
                            var subject = UnitOfWork.SubjectRepository.Find(s => s.Name == cells[row, 6].Value.ToString() && s.SectionId == section.SectionId).FirstOrDefault();
                            var type = UnitOfWork.ScheduleTypeRepository.Find(st => st.Name == cells[row, 7].Value.ToString()).FirstOrDefault();
                            var professor = UnitOfWork.ProfessorRepository.Find(p => p.Name == cells[row, 8].Value.ToString() && p.FacultyId == faculty.FacultyId).FirstOrDefault();
                            var week = ConvertObjectToInt(cells[row, col].Value);

                            if (subgroup != null && subgroup != lastSubgroup)
                            {
                                timeTable = AddTimeTable(subgroup);
                                AddDocumentTimeTable(document, timeTable);
                                lastSubgroup = subgroup;
                            }

                            AddSchedule(type, subject, professor, timeTable, week, day, start, duration);

                            if (cells[++row, 1] == null)
                            {
                                hasValue = false;
                            }
                        }
                        while (hasValue);
                    }
                } 
                catch(Exception ex)
                {
                    throw ex;
                }
                
            }

        }

        public TimeTable AddTimeTable(Subgroup subgroup)
        {
            var timeTable = new TimeTable
            {
                SubgroupId = subgroup.SubgroupId
            };

            UnitOfWork.TimeTableRepository.Add(timeTable);

            Save();

            return timeTable;
        }

        public DocumentTimeTable AddDocumentTimeTable(Document document, TimeTable timeTable)
        {
            var documentTimeTable = new DocumentTimeTable
            {
                DocumentId = document.DocumentId,
                TimeTableId = timeTable.TimeTableId
            };

            UnitOfWork.DocumentTimeTableRepository.Add(documentTimeTable);

            Save();

            return documentTimeTable;
        }

        public Schedule AddSchedule(ScheduleType scheduleType, Subject subject, Professor professor, TimeTable timeTable, int week, string day, string start, float duration)
        {
            var schedule = new Schedule
            {
                ScheduleTypeId = scheduleType.ScheduleTypeId,
                SubjectId = subject.SubjectId,
                ProfessorId = professor.ProfessorId,
                TimeTableId = timeTable.TimeTableId,
                Week = week,
                Day = day,
                StartsAt = start,
                Duration = duration,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            UnitOfWork.ScheduleRepository.Add(schedule);

            Save();

            return schedule;
        }
    }
}
