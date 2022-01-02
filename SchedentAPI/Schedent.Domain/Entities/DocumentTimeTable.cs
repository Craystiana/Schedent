using System.ComponentModel.DataAnnotations;

namespace Schedent.Domain.Entities
{
    public class DocumentTimeTable
    {
        [Key]
        public int DocumentTimeTableId { get; set; }

        [Required]
        public int TimeTableId { get; set; }

        [Required]
        public int DocumentId { get; set; }

        public virtual TimeTable TimeTable { get; set; }
        public virtual Document Document { get; set; }
    }
}
