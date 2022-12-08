using Microsoft.EntityFrameworkCore;
using Schedent.DataAccess.Repositories;
using Schedent.Domain.Entities;
using Schedent.Domain.Interfaces;
using Schedent.Domain.Interfaces.Repositories;
using System;

namespace Schedent.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SchedentContext _context;

        private bool isDisposed;

        // List of repositories
        private IRepository<UserRole> _userRoleRepository;
        private IRepository<Subgroup> _subgroupRepository;
        private IRepository<Group> _groupRepository;
        private IRepository<Section> _sectionRepository;
        private IRepository<Faculty> _facultyRepository;
        private IRepository<Professor> _professorRepository;
        private IRepository<Subject> _subjectRepository;
        private IRepository<Document> _documentRepository;
        private IRepository<TimeTable> _timeTableRepository;
        private IRepository<DocumentTimeTable> _documentTimeTableRepository;
        private IRepository<ScheduleType> _scheduleTypeRepository;
        private INotificationRepository _notificationRepository;
        private IScheduleRepository _scheduleRepository;
        private IUserRepository _userRepository;

        /// <summary>
        /// UnitOfWork constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public UnitOfWork(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SchedentContext>();
            optionsBuilder.UseSqlServer(connectionString);

            _context = new SchedentContext(optionsBuilder.Options);
        }

        /// <summary>
        /// UserRoleRepository getter
        /// </summary>
        public IRepository<UserRole> UserRoleRepository => _userRoleRepository ??= new Repository<UserRole>(_context);
        // <summary>
        /// SubgroupRepository getter
        /// </summary>
        public IRepository<Subgroup> SubgroupRepository => _subgroupRepository ??= new Repository<Subgroup>(_context);
        /// <summary>
        /// GroupRepository getter
        /// </summary>
        public IRepository<Group> GroupRepository => _groupRepository ??= new Repository<Group>(_context);
        /// <summary>
        /// SectionRepository getter
        /// </summary>
        public IRepository<Section> SectionRepository => _sectionRepository ??= new Repository<Section>(_context);
        /// <summary>
        /// FacultyRepository getter
        /// </summary>
        public IRepository<Faculty> FacultyRepository => _facultyRepository ??= new Repository<Faculty>(_context);
        /// <summary>
        /// ProfessorRepository getter
        /// </summary>
        public IRepository<Professor> ProfessorRepository => _professorRepository ??= new Repository<Professor>(_context);
        /// <summary>
        /// SubjectRepository getter
        /// </summary>
        public IRepository<Subject> SubjectRepository => _subjectRepository ??= new Repository<Subject>(_context);
        /// <summary>
        /// DocumentRepository getter
        /// </summary>
        public IRepository<Document> DocumentRepository => _documentRepository ??= new Repository<Document>(_context);
        /// <summary>
        /// TimeTableRepository getter
        /// </summary>
        public IRepository<TimeTable> TimeTableRepository => _timeTableRepository ??= new Repository<TimeTable>(_context);
        /// <summary>
        /// DocumentTimeTableRepository getter
        /// </summary>
        public IRepository<DocumentTimeTable> DocumentTimeTableRepository => _documentTimeTableRepository ??= new Repository<DocumentTimeTable>(_context);
        /// <summary>
        /// ScheduleTypeRepository getter
        /// </summary>
        public IRepository<ScheduleType> ScheduleTypeRepository => _scheduleTypeRepository ??= new Repository<ScheduleType>(_context);
        /// <summary>
        /// NotificationRepository getter
        /// </summary>
        public INotificationRepository NotificationRepository => _notificationRepository ??= new NotificationRepository(_context);
        /// <summary>
        /// ScheduleRepository getter
        /// </summary>
        public IScheduleRepository ScheduleRepository => _scheduleRepository ??= new ScheduleRepository(_context);
        /// <summary>
        /// UserRepository getter
        /// </summary>
        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);

        /// <summary>
        /// Dispose the context
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Save the changes from the context
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
