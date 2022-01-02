using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Schedent.Domain.Entities
{
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }

        [Required]
        public byte[] File { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual ICollection<DocumentTimeTable> DocumentTimeTables { get; set; }
    }
}
