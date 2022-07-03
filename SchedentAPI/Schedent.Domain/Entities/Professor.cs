using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Schedent.Domain.Entities
{
    public class Professor
    {
        [Key]
        public int ProfessorId { get; set; }

        [Required]
        public int FacultyId { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual Faculty Faculty { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual User User { get; set; }
    }
}
