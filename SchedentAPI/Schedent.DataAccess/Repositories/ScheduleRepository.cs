using Microsoft.EntityFrameworkCore;
using Schedent.Domain.Entities;
using Schedent.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Schedent.DataAccess.Repositories
{
    public class ScheduleRepository : Repository<Schedule>, IScheduleRepository
    {
        /// <summary>
        /// ScheduleRepository constructor
        /// Inject the SchedentContext
        /// </summary>
        /// <param name="context"></param>
        public ScheduleRepository(SchedentContext context) : base(context) { }

        /// <summary>
        /// Retrieve the schedules based on the user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Retrieve the schedules based on the professor id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Retrieve the schedules based on the subgroup id
        /// </summary>
        /// <param name="subgroupId"></param>
        /// <returns></returns>
        public IEnumerable<Schedule> GetSchedulesForSubgroup(int subgroupId)
        {
            return _context.TimeTables.Where(t => t.SubgroupId == subgroupId && t.IsActive)
                                      .SelectMany(t => t.Schedules)
                                      .Include(s => s.Professor)
                                      .Include(s => s.ScheduleType)
                                      .Include(s => s.Subject)
                                      .Include(s => s.TimeTable.Subgroup.Group.Section);
        }

        /// <summary>
        /// Retrieve the schedules based on the timetable id
        /// </summary>
        /// <param name="timeTableId"></param>
        /// <returns></returns>
        public IEnumerable<Schedule> GetSchedulesForTimeTable(int timeTableId)
        {
            return _context.Schedules.Where(s => s.TimeTableId == timeTableId)
                                     .Include(s => s.Professor)
                                     .Include(s => s.Subject);
        }
    }
}
