using Microsoft.EntityFrameworkCore;
using Schedent.Domain.Entities;

namespace Schedent.DataAccess
{
    public class SchedentContext : DbContext
    {
        public SchedentContext(DbContextOptions<SchedentContext> options) : base(options) { }

        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Subgroup> Subgroups { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<Faculty> Faculties { get; set;}
        public virtual DbSet<Professor> Professors { get;}
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<TimeTable> TimeTables { get; set; }
        public virtual DbSet<DocumentTimeTable> DocumentTimeTables { get; set; }
        public virtual DbSet<ScheduleType> ScheduleTypes { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Schedule>()
                        .HasOne(sch => sch.Subject)
                        .WithMany(sub => sub.Schedules)
                        .HasForeignKey(sch => sch.SubjectId)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Schedule>()
                        .HasOne(sch => sch.TimeTable)
                        .WithMany(sub => sub.Schedules)
                        .HasForeignKey(sch => sch.TimeTableId)
                        .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}
