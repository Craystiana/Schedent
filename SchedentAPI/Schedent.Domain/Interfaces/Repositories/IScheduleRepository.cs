using Schedent.Domain.Entities;
using System.Collections.Generic;

namespace Schedent.Domain.Interfaces.Repositories
{
    public interface IScheduleRepository : IRepository<Schedule>
    {
        /// <summary>
        /// Retrieve the schedules for the given student
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Schedule> GetSchedulesForStudent(int userId);

        /// <summary>
        /// Retrieve the schedules for the given professor
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Schedule> GetSchedulesForProfessor(int userId);

        /// <summary>
        /// Retrieve the schedules for the given subgroup
        /// </summary>
        /// <param name="subgroupId"></param>
        /// <returns></returns>
        public IEnumerable<Schedule> GetSchedulesForSubgroup(int subgroupId);

        /// <summary>
        /// Retrieve the schedules for the given timetable
        /// </summary>
        /// <param name="timeTableId"></param>
        /// <returns></returns>
        public IEnumerable<Schedule> GetSchedulesForTimeTable(int timeTableId);
    }
}
