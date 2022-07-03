using Schedent.Domain.Entities;
using System.Collections.Generic;

namespace Schedent.Domain.Interfaces.Repositories
{
    public interface INotificationRepository : IRepository<Notification>
    {
        public IEnumerable<Notification> GetNotificationsByIds();
    }
}
