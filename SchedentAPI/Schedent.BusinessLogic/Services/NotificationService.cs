using Schedent.Domain.DTO.Notification;
using Schedent.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Schedent.BusinessLogic.Services
{
    public class NotificationService : BaseService
    {
        public NotificationService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public IEnumerable<Notification> GetUserNotification (int userId)
        {
            var user = UnitOfWork.UserRepository.Get(userId);

            if (user.SubgroupId == null)
            {
                return default;
            }

            return UnitOfWork.NotificationRepository.Find(n => n.SubgroupId == user.SubgroupId && n.IsSent)
                                                    .Select(n => new Notification
                                                    {
                                                        Message = n.Message,
                                                        CreatedOn = n.CreatedOn.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss")
                                                    });
        }
    }
}
