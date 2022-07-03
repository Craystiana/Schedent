namespace Schedent.Domain.DTO.User
{
    public class UserDetails
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public int? FacultyId { get; set; }
        
        public int? SectionId { get; set; }

        public int? GroupId { get; set; }

        public int? SubgroupId { get; set; }
    }
}
