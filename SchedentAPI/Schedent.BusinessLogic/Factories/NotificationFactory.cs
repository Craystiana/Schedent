using Schedent.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Schedent.BusinessLogic.Factories
{
    public class NotificationFactory
    {
        private readonly IDictionary<string, Func<Schedule, Schedule, Notification>> _dict;

        public NotificationFactory()
        {
            _dict = new Dictionary<string, Func<Schedule, Schedule, Notification>>
            {
                { nameof(Schedule.Day), GetDayChangeNotification },
                { nameof(Schedule.Week), GetWeekChangeNotification },
                { nameof(Schedule.ProfessorId), GetProfessorChangeNotification },
                { nameof(Schedule.Duration), GetDurationChangeNotification },
                { nameof(Schedule.StartsAt), GetStartsAtChangeNotification },
            };
        }

        public Notification GetNotification(string property, Schedule oldSchedule, Schedule newSchedule)
        {
            if (_dict.ContainsKey(property))
            {
                return _dict[property](oldSchedule, newSchedule);
            } 
            else
            {
                return null;
            }
        }

        private Notification GetDayChangeNotification(Schedule oldSchedule, Schedule newSchedule)
        {
            return new Notification
            {
                SubgroupId = newSchedule.TimeTable.SubgroupId,
                Message = $"{newSchedule.Subject.Name} moved from {oldSchedule.Day} to {newSchedule.Day}",
                IsSent = false,
                CreatedOn = DateTime.Now,
            };
        }

        private Notification GetWeekChangeNotification(Schedule oldSchedule, Schedule newSchedule)
        {
            static string getWeek(int week)
            {
                if (week == 1)
                {
                    return "odd";
                }
                else if (week == 2)
                {
                    return "even";
                }
                else
                {
                    return "every";
                }
            }

            return new Notification
            {
                SubgroupId = newSchedule.TimeTable.SubgroupId,
                Message = $"{newSchedule.Subject.Name} moved from {getWeek(oldSchedule.Week)} to {getWeek(newSchedule.Week)}",
                IsSent = false,
                CreatedOn = DateTime.Now,
            };
        }

        private Notification GetProfessorChangeNotification(Schedule oldSchedule, Schedule newSchedule)
        {
            return new Notification
            {
                SubgroupId = newSchedule.TimeTable.SubgroupId,
                Message = $"For {newSchedule.Subject.Name} the professor changed from {oldSchedule.Professor.Name} to {newSchedule.Professor.Name}",
                IsSent = false,
                CreatedOn = DateTime.Now,
            };
        }

        private Notification GetDurationChangeNotification(Schedule oldSchedule, Schedule newSchedule)
        {
            return new Notification
            {
                SubgroupId = newSchedule.TimeTable.SubgroupId,
                Message = $"{newSchedule.Subject.Name} now has {newSchedule.Duration}h instead of {oldSchedule.Duration}h",
                IsSent = false,
                CreatedOn = DateTime.Now,
            };
        }

        private Notification GetStartsAtChangeNotification(Schedule oldSchedule, Schedule newSchedule)
        {
            return new Notification
            {
                SubgroupId = newSchedule.TimeTable.SubgroupId,
                Message = $"{newSchedule.Subject.Name} now starts at {newSchedule.StartsAt} instead of {oldSchedule.StartsAt}",
                IsSent = false,
                CreatedOn = DateTime.Now,
            };
        }
    }
}
