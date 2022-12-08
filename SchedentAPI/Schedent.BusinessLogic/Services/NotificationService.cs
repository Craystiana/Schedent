using Schedent.Domain.DTO.Notification;
using Schedent.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Schedent.BusinessLogic.Services
{
    public class NotificationService : BaseService
    {
        /// <summary>
        /// NotificationService constructor
        /// Inject the UnitOfWork
        /// </summary>
        /// <param name="unitOfWork"></param>
        public NotificationService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Retrieve all the notifications assigned to the subgroup of the given user
        /// And map them to the Notification dto
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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
