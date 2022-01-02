using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Schedent.Domain.Entities
{
    public class ScheduleType
    {  
        [Key]
        public int ScheduleTypeId { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
