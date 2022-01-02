using Schedent.Domain.Entities;
using Schedent.Domain.Interfaces.Repositories;
using System;

namespace Schedent.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<UserRole> UserRoleRepository { get; }
        IRepository<Subgroup> SubgroupRepository { get; }
        IRepository<Group> GroupRepository { get; }
        IRepository<Section> SectionRepository { get; }
        IRepository<Faculty> FacultyRepository { get; }
        IRepository<Professor> ProfessorRepository { get; }
        IRepository<Subject> SubjectRepository { get; }
        IRepository<Document> DocumentRepository { get; }
        IRepository<TimeTable> TimeTableRepository { get; }
        IRepository<DocumentTimeTable> DocumentTimeTableRepository { get; }
        IRepository<ScheduleType> ScheduleTypeRepository { get; }
        IRepository<Schedule> ScheduleRepository { get; }
        IUserRepository UserRepository { get; }

        int SaveChanges();
    }
}
