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
        /// <summary>
        /// UserService constructor
        /// Inject UnitOfWork
        /// </summary>
        /// <param name="unitOfWork"></param>
        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
                    ProfessorId = UnitOfWork.ProfessorRepository.Find(p => p.Name == model.FirstName + " " + model.LastName).FirstOrDefault()?.ProfessorId,
                    UserRoleId = model.Subgroup != null ? (int)UserRoleType.Student : (int)UserRoleType.Professor,
                    Salt = salt,
                    PasswordHash = CreatePasswordHash(model.Password, salt)
                };

                UnitOfWork.UserRepository.Add(user);
            }

            Save();

            return user;
        }

        /// <summary>
        /// Login the user based on the username and password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Generate a byte array salt and convert it to string
        /// </summary>
        /// <returns></returns>
        private static string GenerateSalt()
        {
            byte[] salt = new byte[50 / 8];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }

        /// <summary>
        /// Generate a hash for the password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private static string CreatePasswordHash(string password, string salt)
        {
            var sha1 = SHA1.Create();
            var hashedBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }

        /// <summary>
        /// Return the user details based on the given user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserDetails GetUserDetails(int userId)
        {
            var user = UnitOfWork.UserRepository.GetDetails(userId);

            return new UserDetails
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.Email,
                FacultyId = user.Subgroup?.Group.Section.FacultyId,
                SectionId = user.Subgroup?.Group.SectionId,
                GroupId = user.Subgroup?.GroupId,
                SubgroupId = user.SubgroupId,
            };
        }

        /// <summary>
        /// Update user data
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        public void EditProfile(UserDetails model, int userId)
        {
            var user = UnitOfWork.UserRepository.Get(userId);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.EmailAddress;
            user.SubgroupId = model.SubgroupId;

            UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// Update the device token of the given user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        public void EditDeviceToken(int userId, string token)
        {
            var user = UnitOfWork.UserRepository.Get(userId);

            user.DeviceToken = token;

            UnitOfWork.SaveChanges();
        }
    }
}
