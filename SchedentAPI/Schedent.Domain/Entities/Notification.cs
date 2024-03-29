﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Schedent.Domain.Entities
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        public int? SubgroupId { get; set; }

        public int? ProfessorId { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public bool IsSent { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public virtual Subgroup Subgroup { get; set; }

        public virtual Professor Professor { get; set; }
    }
}
