using System;
using System.ComponentModel.DataAnnotations;

namespace Schedent.Domain.Entities
{
    public class Schedule
    {
        [Key]
        public int ScheduleId { get; set; }

        [Required]
        public int ScheduleTypeId { get; set; }

        [Required]
        public int SubjectId { get; set; }

        [Required]
        public int ProfessorId { get; set; }

        [Required]
        public int TimeTableId { get; set; }

        public int Week { get; set; }

        [Required]
        public string Day { get; set; }

        [Required]
        public float Duration { get; set; }

        [Required]
        public string StartsAt { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public DateTime UpdatedOn { get; set; }

        public virtual ScheduleType ScheduleType { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Professor Professor { get; set; }
        public virtual TimeTable TimeTable { get; set; }
    }
}
