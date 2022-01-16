namespace Schedent.Domain.DTO.User
{
    public class RegisterModel : LoginModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Subgroup { get; set; }
    }
}
