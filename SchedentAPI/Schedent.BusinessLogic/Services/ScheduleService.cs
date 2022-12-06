﻿using Schedent.Common.Enums;
using Schedent.Domain.DTO.Schedule;
using Schedent.Domain.Entities;
using Schedent.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Schedent.BusinessLogic.Services
{
    public class ScheduleService : BaseService
    {
        public ScheduleService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public IEnumerable<ScheduleListModel> GetUserTimeTable(int userId, int userRoleId)
        {
            var schedules = userRoleId == (int)UserRoleType.Student ? UnitOfWork.ScheduleRepository.GetSchedulesForStudent(userId) : UnitOfWork.ScheduleRepository.GetSchedulesForProfessor(userId);

            return GroupAndOrderSchedules(schedules);
        }

        public IEnumerable<ScheduleListModel> GetSubgroupTimeTable(int subgroupId)
        {
            var schedules = UnitOfWork.ScheduleRepository.GetSchedulesForSubgroup(subgroupId);

            return GroupAndOrderSchedules(schedules);
        }

        public static IEnumerable<ScheduleListModel> GroupAndOrderSchedules(IEnumerable<Schedule> schedules)
        {
            return schedules.GroupBy(s => s.Day).Select(s => new ScheduleListModel
            {
                Day = s.Key,
                Schedules = s.Select(s => new ScheduleModel
                {
                    ScheduleType = s.ScheduleType.Name,
                    Subject = s.Subject.Name,
                    Professor = s.Professor.Name,
                    Subgroup = s.TimeTable.Subgroup.Group.Name + s.TimeTable.Subgroup.Name,
                    Week = s.Week,
                    Duration = s.Duration,
                    StartsAt = s.StartsAt
                })
            }).OrderBy(s => Enum.GetNames(typeof(WeekDays)).ToList().IndexOf(s.Day));
        }
    }
}
