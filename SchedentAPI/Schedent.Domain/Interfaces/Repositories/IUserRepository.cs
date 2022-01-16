using Schedent.Domain.Entities;
using System;

namespace Schedent.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        public User Get(string email);
    }
}
