using Microsoft.EntityFrameworkCore;
using Schedent.Domain.Entities;
using Schedent.Domain.Interfaces.Repositories;
using System.Linq;

namespace Schedent.DataAccess.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        /// <summary>
        /// UserRepository constructor
        /// </summary>
        /// <param name="context"></param>
        public UserRepository(SchedentContext context) : base(context) { }

        /// <summary>
        /// Retrieve the user by the email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public User Get(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        /// <summary>
        /// Retrieve the user details by the user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetDetails(int userId)
        {
            return _context.Users.Include(u => u.Subgroup)
                                 .ThenInclude(sg => sg.Group)
                                 .ThenInclude(g => g.Section)
                                 .ThenInclude(s => s.Faculty)
                                 .SingleOrDefault(u => u.UserId == userId);

        }
    }
}
