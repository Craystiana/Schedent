using Schedent.Domain.Entities;
using System.Collections.Generic;

namespace Schedent.Domain.Interfaces.Repositories
{
    public interface INotificationRepository : IRepository<Notification>
    {
        /// <summary>
        /// Retrieve the unsent notifications
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Notification> GetNotificationsByIds();
    }
}
