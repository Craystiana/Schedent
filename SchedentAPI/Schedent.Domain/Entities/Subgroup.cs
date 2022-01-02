using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Schedent.Domain.Entities
{
    public class Subgroup
    {
        [Key]
        public int SubgroupId { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual Group Group { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<TimeTable> TimeTables { get; set; }
    }
}
