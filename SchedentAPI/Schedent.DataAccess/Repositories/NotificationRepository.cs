using Microsoft.EntityFrameworkCore;
using Schedent.Domain.Entities;
using Schedent.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Schedent.DataAccess.Repositories
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(SchedentContext context) : base(context) { }

        public IEnumerable<Notification> GetNotificationsByIds()
        {
            return _context.Notifications.Include(n => n.Subgroup)
                                         .ThenInclude(s => s.Users)
                                         .Include(n => n.Professor)
                                         .ThenInclude(p => p.User)
                                         .Where(n => !n.IsSent)
                                         .ToList();
        }
    }
}
