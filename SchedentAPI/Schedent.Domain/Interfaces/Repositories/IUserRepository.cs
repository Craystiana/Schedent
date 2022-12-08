using Schedent.Domain.Entities;

namespace Schedent.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Retrieve the user by the email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public User Get(string email);
        /// <summary>
        /// Retrieve the user details by the id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetDetails(int userId);
    }
}
