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
        private IRepository<Notification> _notificationRepository;
        private IScheduleRepository _scheduleRepository;
        private IUserRepository _userRepository;

        public UnitOfWork(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SchedentContext>();
            optionsBuilder.UseSqlServer(connectionString);

            _context = new SchedentContext(optionsBuilder.Options);
        }

        public IRepository<UserRole> UserRoleRepository => _userRoleRepository ??= new Repository<UserRole>(_context);
        public IRepository<Subgroup> SubgroupRepository => _subgroupRepository ??= new Repository<Subgroup>(_context);
        public IRepository<Group> GroupRepository => _groupRepository ??= new Repository<Group>(_context); 
        public IRepository<Section> SectionRepository => _sectionRepository ??= new Repository<Section>(_context);
        public IRepository<Faculty> FacultyRepository => _facultyRepository ??= new Repository<Faculty>(_context);
        public IRepository<Professor> ProfessorRepository => _professorRepository ??= new Repository<Professor>(_context);
        public IRepository<Subject> SubjectRepository => _subjectRepository ??= new Repository<Subject>(_context);
        public IRepository<Document> DocumentRepository => _documentRepository ??= new Repository<Document>(_context);
        public IRepository<TimeTable> TimeTableRepository => _timeTableRepository ??= new Repository<TimeTable>(_context);
        public IRepository<DocumentTimeTable> DocumentTimeTableRepository => _documentTimeTableRepository ??= new Repository<DocumentTimeTable>(_context);
        public IRepository<ScheduleType> ScheduleTypeRepository => _scheduleTypeRepository ??= new Repository<ScheduleType>(_context);
        public IRepository<Notification> NotificationRepository => _notificationRepository ??= new Repository<Notification>(_context);
        public IScheduleRepository ScheduleRepository => _scheduleRepository ??= new ScheduleRepository(_context);
        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);

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

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
