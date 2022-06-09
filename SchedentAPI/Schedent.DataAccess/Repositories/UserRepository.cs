using Microsoft.EntityFrameworkCore;
using Schedent.Domain.Entities;
using Schedent.Domain.Interfaces.Repositories;
using System.Linq;

namespace Schedent.DataAccess.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(SchedentContext context) : base(context) { }

        public User Get(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

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
