using Schedent.Domain.DTO.Generic;
using Schedent.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Schedent.BusinessLogic.Services
{
    public class FacultyService : BaseService
    {
        public FacultyService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public IEnumerable<GenericModel> GetListOfFaculties()
        {
            return UnitOfWork.FacultyRepository.GetAll()
                                               .Select(f => new GenericModel
                                               {
                                                   Id = f.FacultyId,
                                                   Name = f.Name,
                                               });
        }
    }
}
