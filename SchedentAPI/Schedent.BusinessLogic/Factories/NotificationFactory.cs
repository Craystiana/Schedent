using Schedent.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Schedent.BusinessLogic.Factories
{
    public class NotificationFactory
    {
        private readonly IDictionary<string, Func<Schedule, Schedule, IEnumerable<Notification>>> _dict;

        /// <summary>
        /// NotificationFactory constructor
        /// </summary>
        public NotificationFactory()
        {
            _dict = new Dictionary<string, Func<Schedule, Schedule, IEnumerable<Notification>>>
            {
                { nameof(Schedule.Day), GetDayChangeNotification },
                { nameof(Schedule.Week), GetWeekChangeNotification },
                { nameof(Schedule.ProfessorId), GetProfessorChangeNotification },
                { nameof(Schedule.Duration), GetDurationChangeNotification },
                { nameof(Schedule.StartsAt), GetStartsAtChangeNotification },
            };
        }

        /// <summary>
        /// Calls the corresponding notification method by the given property
        /// </summary>
        /// <param name="property"></param>
        /// <param name="oldSchedule"></param>
        /// <param name="newSchedule"></param>
        /// <returns></returns>
        public IEnumerable<Notification> GetNotification(string property, Schedule oldSchedule, Schedule newSchedule)
        {
            if (_dict.ContainsKey(property))
            {
                return _dict[property](oldSchedule, newSchedule);
            } 
            else
            {
                return Enumerable.Empty<Notification>();
            }
        }

        /// <summary>
        /// Create notifications for the change of the day in the schedule
        /// </summary>
        /// <param name="oldSchedule"></param>
        /// <param name="newSchedule"></param>
        /// <returns></returns>
        private IEnumerable<Notification> GetDayChangeNotification(Schedule oldSchedule, Schedule newSchedule)
        {
            return new List<Notification>()
            {
                new Notification 
                {
                    SubgroupId = newSchedule.TimeTable.SubgroupId,
                    Message = $"{newSchedule.ScheduleType.Name}ul {newSchedule.Subject.Name} s-a mutat din ziua de {oldSchedule.Day} în ziua de {newSchedule.Day}",
                    IsSent = false,
                    CreatedOn = DateTime.Now
                },
                new Notification
                {
                    ProfessorId = newSchedule.ProfessorId,
                    Message = $"{newSchedule.ScheduleType.Name}ul {newSchedule.Subject.Name} s-a mutat din ziua de {oldSchedule.Day} în ziua de {newSchedule.Day}",
                    IsSent = false,
                    CreatedOn = DateTime.Now
                }
            };
        }

        /// <summary>
        /// Create notifications for the change of the week in the schedule
        /// </summary>
        /// <param name="oldSchedule"></param>
        /// <param name="newSchedule"></param>
        /// <returns></returns>
        private IEnumerable<Notification> GetWeekChangeNotification(Schedule oldSchedule, Schedule newSchedule)
        {
            static string getWeek(int week)
            {
                if (week == 1)
                {
                    return "impară";
                }
                else if (week == 2)
                {
                    return "pară";
                }
                else
                {
                    return "recurenta";
                }
            }

            return new List<Notification>()
            {
                new Notification
                {
                    SubgroupId = newSchedule.TimeTable.SubgroupId,
                    Message = $"{newSchedule.ScheduleType.Name}ul {newSchedule.Subject.Name} s-a mutat din săptămână {getWeek(oldSchedule.Week)} în săptămână {getWeek(newSchedule.Week)}",
                    IsSent = false,
                    CreatedOn = DateTime.Now,
                },
                new Notification
                {
                    ProfessorId = newSchedule.ProfessorId,
                    Message = $"{newSchedule.ScheduleType.Name}ul {newSchedule.Subject.Name} s-a mutat din săptămână {getWeek(oldSchedule.Week)} în săptămână {getWeek(newSchedule.Week)}",
                    IsSent = false,
                    CreatedOn = DateTime.Now,
                }
            };
        }

        /// <summary>
        /// Create notifications for the change of the professor in the schedule
        /// </summary>
        /// <param name="oldSchedule"></param>
        /// <param name="newSchedule"></param>
        /// <returns></returns>
        private IEnumerable<Notification> GetProfessorChangeNotification(Schedule oldSchedule, Schedule newSchedule)
        {
            return new List<Notification>()
            {
                new Notification 
                {
                    SubgroupId = newSchedule.TimeTable.SubgroupId,
                    Message = $"Pentru {newSchedule.ScheduleType.Name}ul {newSchedule.Subject.Name} profesorul {oldSchedule.Professor.Name} a fost schimbat cu profesorul {newSchedule.Professor.Name}",
                    IsSent = false,
                    CreatedOn = DateTime.Now
                },
                new Notification 
                {
                    ProfessorId = newSchedule.ProfessorId,
                    Message = $"Pentru {newSchedule.ScheduleType.Name}ul {newSchedule.Subject.Name} profesorul {oldSchedule.Professor.Name} a fost schimbat cu profesorul {newSchedule.Professor.Name}",
                    IsSent = false,
                    CreatedOn = DateTime.Now
                }

            };
        }

        /// <summary>
        /// Create notifications for the change of the duration in the schedule
        /// </summary>
        /// <param name="oldSchedule"></param>
        /// <param name="newSchedule"></param>
        /// <returns></returns>
        private IEnumerable<Notification> GetDurationChangeNotification(Schedule oldSchedule, Schedule newSchedule)
        {
            return new List<Notification>()
            {
                new Notification
                {
                    SubgroupId = newSchedule.TimeTable.SubgroupId,
                    Message = $"{newSchedule.ScheduleType.Name}ul {newSchedule.Subject.Name} are de acum {newSchedule.Duration}h în loc de {oldSchedule.Duration}h",
                    IsSent = false,
                    CreatedOn = DateTime.Now
                },
                new Notification{
                    ProfessorId = newSchedule.ProfessorId,
                    Message = $"{newSchedule.ScheduleType.Name}ul {newSchedule.Subject.Name} are de acum {newSchedule.Duration}h în loc de {oldSchedule.Duration}h",
                    IsSent = false,
                    CreatedOn = DateTime.Now
                }
            };
        }

        /// <summary>
        /// Create notifications for the change of the staring time in the schedule
        /// </summary>
        /// <param name="oldSchedule"></param>
        /// <param name="newSchedule"></param>
        /// <returns></returns>
        private IEnumerable<Notification> GetStartsAtChangeNotification(Schedule oldSchedule, Schedule newSchedule)
        {
            return new List<Notification>()
            {
                new Notification
                {
                    SubgroupId = newSchedule.TimeTable.SubgroupId,
                    Message = $"{newSchedule.ScheduleType.Name}ul {newSchedule.Subject.Name} începe de acum la ora {newSchedule.StartsAt} în loc de ora {oldSchedule.StartsAt}",
                    IsSent = false,
                    CreatedOn = DateTime.Now
                },
                new Notification
                {
                    ProfessorId = newSchedule.ProfessorId,
                    Message = $"{newSchedule.ScheduleType.Name}ul {newSchedule.Subject.Name} începe de acum la ora {newSchedule.StartsAt} în loc de ora {oldSchedule.StartsAt}",
                    IsSent = false,
                    CreatedOn = DateTime.Now
                }
            };
        }
    }
}
