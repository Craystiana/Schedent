using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Schedent.Domain.Entities
{
    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }

        [Required]
        public int SectionId { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual Section Section { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
