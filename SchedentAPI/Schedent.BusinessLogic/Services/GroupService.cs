using Schedent.Domain.DTO.Generic;
using Schedent.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Schedent.BusinessLogic.Services
{
    public class GroupService : BaseService
    {
        public GroupService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public IEnumerable<GenericModel> GetListOfGroups(int sectionId)
        {
            return UnitOfWork.GroupRepository.Find(g => g.SectionId == sectionId)
                                             .Select(g => new GenericModel
                                             {
                                                 Id = g.GroupId,
                                                 Name = g.Name,
                                             });
        }
    }
}
