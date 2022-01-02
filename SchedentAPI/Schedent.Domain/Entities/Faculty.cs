using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Schedent.Domain.Entities
{
    public class Faculty
    {
        [Key]
        public int FacultyId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public virtual ICollection<Section> Sections { get; set; }
        public virtual ICollection<Professor> Professors { get; set;}
    }
}
