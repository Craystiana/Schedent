using Schedent.Domain.Interfaces;

namespace Schedent.BusinessLogic.Services
{
    public abstract class BaseService
    {
        private protected readonly IUnitOfWork UnitOfWork;

        protected BaseService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        private protected void Save()
        {
            UnitOfWork.SaveChanges();
        }
    }
}
