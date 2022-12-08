using Schedent.Domain.DTO.Generic;
using Schedent.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Schedent.BusinessLogic.Services
{
    public class SectionService : BaseService
    {
        /// <summary>
        /// SectionService constructor
        /// Inject the UnitOfWork
        /// </summary>
        /// <param name="unitOfWork"></param>
        public SectionService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Retrieve the list of sections based on the given faculty
        /// And map them to the GenericModel
        /// </summary>
        /// <param name="facultyId"></param>
        /// <returns></returns>
        public IEnumerable<GenericModel> GetListOfSections(int facultyId)
        {
            return UnitOfWork.SectionRepository.Find(s => s.FacultyId == facultyId)
                                               .Select(s => new GenericModel
                                               {
                                                   Id = s.SectionId,
                                                   Name = s.Name,
                                               });
        }
    }
}
