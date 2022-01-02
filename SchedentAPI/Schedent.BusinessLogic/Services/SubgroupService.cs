using Schedent.Domain.DTO.Generic;
using Schedent.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Schedent.BusinessLogic.Services
{
    public class SubgroupService : BaseService
    {
        public SubgroupService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public IEnumerable<GenericModel> GetListOfSubgroups(int groupId)
        {
            return UnitOfWork.SubgroupRepository.Find(sg => sg.GroupId == groupId)
                                                .Select(sg => new GenericModel
                                                {
                                                    Id = sg.SubgroupId,
                                                    Name = sg.Name,
                                                });
        }
    }
}
