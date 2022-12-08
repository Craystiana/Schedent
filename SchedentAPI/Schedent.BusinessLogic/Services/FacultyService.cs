using Schedent.Domain.DTO.Generic;
using Schedent.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Schedent.BusinessLogic.Services
{
    public class FacultyService : BaseService
    {
        /// <summary>
        /// FacultyService constructor
        /// Inject the UnitOfWork
        /// </summary>
        /// <param name="unitOfWork"></param>
        public FacultyService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Method for retrieving the list of faculties frfom the database
        /// And converting them to the GenericModel
        /// </summary>
        /// <returns></returns>
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
