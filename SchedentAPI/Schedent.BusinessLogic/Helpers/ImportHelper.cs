using FirebaseAdmin.Messaging;
using Google.Apis.Calendar.v3.Data;
using Schedent.BusinessLogic.Factories;
using Schedent.Common.Enums;
using Schedent.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schedent.BusinessLogic.Helpers
{
    public static class ImportHelper
    {
        /// <summary>
        /// Set the notifications
        /// </summary>
        /// <param name="notifications"></param>
        /// <param name="notificationFactory"></param>
        /// <param name="newSchedule"></param>
        /// <param name="oldSchedule"></param>
        public static void AddNotifications(List<Domain.Entities.Notification> notifications, NotificationFactory notificationFactory, Schedule newSchedule, Schedule oldSchedule)
        {
            if (newSchedule.ProfessorId != oldSchedule.ProfessorId)
            {
                notifications.AddRange(notificationFactory.GetNotification(nameof(newSchedule.ProfessorId), oldSchedule, newSchedule));
            }

            if (newSchedule.Day != oldSchedule.Day)
            {
                notifications.AddRange(notificationFactory.GetNotification(nameof(newSchedule.Day), oldSchedule, newSchedule));
            }

            if (newSchedule.Duration != oldSchedule.Duration)
            {
                notifications.AddRange(notificationFactory.GetNotification(nameof(newSchedule.Duration), oldSchedule, newSchedule));
            }

            if (newSchedule.StartsAt != oldSchedule.StartsAt)
            {
                notifications.AddRange(notificationFactory.GetNotification(nameof(newSchedule.StartsAt), oldSchedule, newSchedule));
            }

            if (newSchedule.Week != oldSchedule.Week)
            {
                notifications.AddRange(notificationFactory.GetNotification(nameof(newSchedule.Week), oldSchedule, newSchedule));
            }
        }

        /// <summary>
        /// Send the notifications to the users
        /// </summary>
        /// <param name="notifications"></param>
        /// <returns></returns>
        public static async Task SendFirebaseNotificationsAsync(IEnumerable<Domain.Entities.Notification> notifications)
        {
            foreach (var notification in notifications)
            {
                if ((notification.Subgroup?.Users != null && notification.Subgroup?.Users.Count != 0) || notification.Professor?.User != null)
                {
                    var message = new MulticastMessage
                    {
                        Tokens = notification.Subgroup != null ?
                                 notification.Subgroup.Users.Select(u => u.DeviceToken).ToList() :
                                 new List<string>() { notification.Professor.User.DeviceToken },
                        Notification = new FirebaseAdmin.Messaging.Notification
                        {
                            Title = "Orarul tău a fost modificat",
                            Body = notification.Message
                        },
                    };

                    await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
                    notification.IsSent = true;
                }
            }
        }

        /// <summary>
        /// Create a new event entity
        /// </summary>
        /// <param name="schedule"></param>
        /// <param name="semDayStart"></param>
        /// <param name="semMonthStart"></param>
        /// <param name="yearStart"></param>
        /// <param name="recurrence"></param>
        /// <param name="atendeeEmails"></param>
        /// <returns></returns>
        public static Event CreateEventEntity(Schedule schedule, string semDayStart, string semMonthStart, string yearStart, string recurrence, List<EventAttendee> atendeeEmails)
        {
            return new Event
            {
                Summary = schedule.Subject.Name,
                Organizer = new Event.OrganizerData
                {
                    DisplayName = schedule.Professor.Name,
                },
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
                Recurrence = new string[] { recurrence },
                Attendees = atendeeEmails
            };
        }
    }
}
