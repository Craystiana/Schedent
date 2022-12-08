using Schedent.Domain.DTO.Generic;
using Schedent.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Schedent.BusinessLogic.Services
{
    public class GroupService : BaseService
    {
        /// <summary>
        /// GroupService constructor
        /// Inject the UnitOfWork
        /// </summary>
        /// <param name="unitOfWork"></param>
        public GroupService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Method for retrieving the list of groups from the database based on section
        /// And converting them to the GenericModel
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
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