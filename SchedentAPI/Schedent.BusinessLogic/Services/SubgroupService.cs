using Schedent.Domain.DTO.Generic;
using Schedent.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Schedent.BusinessLogic.Services
{
    public class SubgroupService : BaseService
    {
        /// <summary>
        /// SubgroupService constructor
        /// Inject the UnitOfWork
        /// </summary>
        /// <param name="unitOfWork"></param>
        public SubgroupService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Retrieve the list of subgroups based on the given group
        /// And map them to the GenericModel
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
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
