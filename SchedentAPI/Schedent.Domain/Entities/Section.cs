using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Schedent.Domain.Entities
{
    public class Section
    {
        [Key]
        public int SectionId { get; set; }

        [Required]
        public int FacultyId { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual Faculty Faculty { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
