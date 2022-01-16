using Schedent.Domain.Entities;
using System.Collections.Generic;

namespace Schedent.Domain.Interfaces.Repositories
{
    public interface IScheduleRepository : IRepository<Schedule>
    {
        public IEnumerable<Schedule> GetSchedulesForStudent(int userId);

        public IEnumerable<Schedule> GetSchedulesForProfessor(int userId);

        public IEnumerable<Schedule> GetSchedulesForSubgroup(int subgroupId);
    }
}
