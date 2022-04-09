﻿using Microsoft.EntityFrameworkCore;
using Schedent.Domain.Entities;
using Schedent.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Schedent.DataAccess.Repositories
{
    public class ScheduleRepository : Repository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(SchedentContext context) : base(context) { }

        public IEnumerable<Schedule> GetSchedulesForStudent(int userId)
        {
            return _context.Users.Where(u => u.UserId == userId)
                                 .Select(u => u.Subgroup)
                                 .SelectMany(s => s.TimeTables)
                                 .Where(t => t.IsActive)
                                 .SelectMany(t => t.Schedules)
                                 .Include(s => s.Professor)
                                 .Include(s => s.ScheduleType)
                                 .Include(s => s.Subject)
                                 .Include(s => s.TimeTable.Subgroup.Group.Section);
        }

        public IEnumerable<Schedule> GetSchedulesForProfessor(int userId)
        {
            return _context.Users.Where(u => u.UserId == userId)
                                 .Select(u => u.Professor)
                                 .SelectMany(p => p.Schedules)
                                 .Where(s => s.TimeTable.IsActive)
                                 .Include(s => s.Professor)
                                 .Include(s => s.ScheduleType)
                                 .Include(s => s.Subject)
                                 .Include(s => s.TimeTable.Subgroup.Group.Section);
        }

        public IEnumerable<Schedule> GetSchedulesForSubgroup(int subgroupId)
        {
            return _context.TimeTables.Where(t => t.SubgroupId == subgroupId && t.IsActive)
                                      .SelectMany(t => t.Schedules)
                                      .Include(s => s.Professor)
                                      .Include(s => s.ScheduleType)
                                      .Include(s => s.Subject)
                                      .Include(s => s.TimeTable.Subgroup.Group.Section);
        }

        public IEnumerable<Schedule> GetSchedulesForTimeTable(int timeTableId)
        {
            return _context.Schedules.Where(s => s.TimeTableId == timeTableId)
                                     .Include(s => s.Professor)
                                     .Include(s => s.Subject);
        }
    }
}
