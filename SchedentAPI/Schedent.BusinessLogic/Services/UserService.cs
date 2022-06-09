using Schedent.Common.Enums;
using Schedent.Domain.DTO.User;
using Schedent.Domain.Entities;
using Schedent.Domain.Interfaces;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Schedent.BusinessLogic.Services
{
    public class UserService : BaseService
    {
        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public User Register(RegisterModel model)
        {
            User user = null;

            model.Email = model.Email.Replace(" ", string.Empty);

            if (UnitOfWork.UserRepository.Get(model.Email) == null)
            {
                var salt = GenerateSalt();

                user = new User
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    SubgroupId = model.Subgroup,
                    ProfessorId = UnitOfWork.ProfessorRepository.Find(p => p.Name == model.FirstName + " " + model.LastName).FirstOrDefault().ProfessorId,
                    UserRoleId = model.Subgroup != null ? (int)UserRoleType.Student : (int)UserRoleType.Professor,
                    Salt = salt,
                    PasswordHash = CreatePasswordHash(model.Password, salt)
                };

                UnitOfWork.UserRepository.Add(user);
            }

            Save();

            return user;
        }

        public User Login(string email, string password)
        {
            var user = UnitOfWork.UserRepository.Get(email.Replace(" ", string.Empty));

            if (user?.PasswordHash == CreatePasswordHash(password, user?.Salt))
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        private string GenerateSalt()
        {
            byte[] salt = new byte[50 / 8];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }

        private string CreatePasswordHash(string password, string salt)
        {
            using var sha1 = SHA1.Create();
            {
                var hashedBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public UserDetails GetUserDetails(int userId)
        {
            var user = UnitOfWork.UserRepository.GetDetails(userId);

            return new UserDetails
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.Email,
                FacultyId = user.Subgroup.Group.Section.FacultyId,
                SectionId = user.Subgroup.Group.SectionId,
                GroupId = user.Subgroup.GroupId,
                SubgroupId = (int)user.SubgroupId,
            };
        }

        public void EditProfile(UserDetails model, int userId)
        {
            var user = UnitOfWork.UserRepository.Get(userId);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.EmailAddress;
            user.SubgroupId = model.SubgroupId;

            UnitOfWork.SaveChanges();
        }
    }
}
