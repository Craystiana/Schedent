using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Schedent.Domain.Entities
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }

        [Required]
        public int SectionId { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual Section Section { get; set; }
        public virtual ICollection<Subgroup> Subgroups { get; set; }
    }
}
