using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Schedent.Domain.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public int UserRoleId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public int? SubgroupId { get; set; }

        public int? ProfessorId { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Salt { get; set; }

        public string DeviceToken { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool EventsSent { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public DateTime UpdatedOn { get; set; }

        public virtual UserRole UserRole { get; set; }
        public virtual Subgroup Subgroup { get; set; }
        public virtual Professor Professor { get; set; }
    }
}
