using Schedent.Domain.Entities;

namespace Schedent.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        public User Get(string email);
    }
}
