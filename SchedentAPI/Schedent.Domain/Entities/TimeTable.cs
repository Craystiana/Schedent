using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Schedent.Domain.Entities
{
    public class TimeTable
    {   
        [Key]
        public int TimeTableId { get; set; }

        [Required]
        public int SubgroupId { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual Subgroup Subgroup { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<DocumentTimeTable> DocumentTimeTables { get; set; }
    }
}
