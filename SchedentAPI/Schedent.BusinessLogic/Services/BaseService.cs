using Schedent.Domain.Interfaces;

namespace Schedent.BusinessLogic.Services
{
    public abstract class BaseService
    {
        private protected readonly IUnitOfWork UnitOfWork;

        /// <summary>
        /// BaseService constructor
        /// Inject the UnitOfWork
        /// </summary>
        /// <param name="unitOfWork"></param>
        protected BaseService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        /// <summary>
        /// Save the changes of the current context
        /// </summary>
        private protected void Save()
        {
            UnitOfWork.SaveChanges();
        }
    }
}
