using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Schedent.Common.Enums;
using Schedent.Domain.Entities;
using Schedent.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Schedent.BusinessLogic.Services
{
    public class GoogleCalendarService : BaseService
    {
        public GoogleCalendarService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public CalendarService GetService()
        {
            string[] Scopes = { CalendarService.Scope.CalendarReadonly };
            string ApplicationName = "Google Calendar API .NET Quickstart";
            UserCredential credential;

            using (var stream = new FileStream("desktop_calendar_client_secret.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, Scopes, "user", CancellationToken.None, new FileDataStore(credPath, true)).Result;
            }

            // Create Google Calendar API service.
            return new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }

        public void GetUserSchedules()
        {
            var service = GetService();

            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            Events events = request.Execute();
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    string when = eventItem.Start.DateTime.ToString();
                    if (String.IsNullOrEmpty(when))
                    {
                        when = eventItem.Start.Date;
                    }
                }
            }
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
                                                         })
                                                         .ToList();

            var ev = new Event
            {
                Summary = schedule.Subject.Name,
                Start = new EventDateTime
                {
                    DateTime = Convert.ToDateTime((Int32.Parse(semDayStart) + (int)Enum.Parse(typeof(WeekDays), schedule.Day)).ToString() + "/" + semMonthStart + "/" + yearStart + " " + schedule.StartsAt)
                },
                End = new EventDateTime
                {
                    DateTime = Convert.ToDateTime((Int32.Parse(semDayStart) + (int)Enum.Parse(typeof(WeekDays), schedule.Day)).ToString() + "/" + semMonthStart + "/" + yearStart + " " + (Int32.Parse(schedule.StartsAt) + schedule.Duration).ToString())
                },
                Recurrence = new String[] 
                {
                    "RRULE:FREQ=WEEKLY;UNTIL=20220805T200000Z"
                },
                Attendees = atendeeEmails
            };

            Event recurringEvent = GetService().Events.Insert(ev, "primary").Execute();
        }
    }
}
