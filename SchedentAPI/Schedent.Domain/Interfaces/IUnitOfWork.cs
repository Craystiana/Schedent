using Schedent.Domain.Entities;
using Schedent.Domain.Interfaces.Repositories;
using System;

namespace Schedent.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// UserRoleRepository getter
        /// </summary>
        IRepository<UserRole> UserRoleRepository { get; }
        /// <summary>
        /// SubgroupRepository getter
        /// </summary>
        IRepository<Subgroup> SubgroupRepository { get; }
        /// <summary>
        /// GroupRepository getter
        /// </summary>
        IRepository<Group> GroupRepository { get; }
        /// <summary>
        /// SectionRepository getter
        /// </summary>
        IRepository<Section> SectionRepository { get; }
        /// <summary>
        /// FacultyRepository getter
        /// </summary>
        IRepository<Faculty> FacultyRepository { get; }
        /// <summary>
        /// ProfessorRepository getter
        /// </summary>
        IRepository<Professor> ProfessorRepository { get; }
        /// <summary>
        /// SubjectRepository getter
        /// </summary>
        IRepository<Subject> SubjectRepository { get; }
        /// <summary>
        /// DocumentRepository getter
        /// </summary>
        IRepository<Document> DocumentRepository { get; }
        /// <summary>
        /// TimeTableRepository getter
        /// </summary>
        IRepository<TimeTable> TimeTableRepository { get; }
        /// <summary>
        /// DocumentTimeTableRepository getter
        /// </summary>
        IRepository<DocumentTimeTable> DocumentTimeTableRepository { get; }
        /// <summary>
        /// ScheduleTypeRepository getter
        /// </summary>
        IRepository<ScheduleType> ScheduleTypeRepository { get; }
        /// <summary>
        /// NotificationRepository getter
        /// </summary>
        INotificationRepository NotificationRepository { get; }
        /// <summary>
        /// ScheduleRepository getter
        /// </summary>
        IScheduleRepository ScheduleRepository { get; }
        /// <summary>
        /// UserRepository getter
        /// </summary>
        IUserRepository UserRepository { get; }

        /// <summary>
        /// Save the changes in the current context
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
    }
}
