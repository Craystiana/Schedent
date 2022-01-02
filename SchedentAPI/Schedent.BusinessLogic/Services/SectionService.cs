using Schedent.Domain.DTO.Generic;
using Schedent.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Schedent.BusinessLogic.Services
{
    public class SectionService : BaseService
    {
        public SectionService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

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
