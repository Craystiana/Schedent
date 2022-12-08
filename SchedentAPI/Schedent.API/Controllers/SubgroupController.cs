using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Schedent.BusinessLogic.Services;
using System;
using System.Net;

namespace Schedent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubgroupController : BaseController
    {
        private readonly SubgroupService _subgroupService;
        private readonly ILogger<SubgroupController> _logger;

        /// <summary>
        /// SubgroupController constructor
        /// Inject the SubgroupService and the logger
        /// </summary>
        /// <param name="subgroupService"></param>
        /// <param name="logger"></param>
        public SubgroupController(SubgroupService subgroupService, ILogger<SubgroupController> logger)
        {
            _subgroupService = subgroupService;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint for retrieving all the subgroups of the given group
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("All")]
        public IActionResult All([FromQuery] int groupId)
        {
            try
            {
                return new JsonResult(_subgroupService.GetListOfSubgroups(groupId));
            }
            catch(Exception ex)
            {
                _logger.LogError("An error occured: {error}", ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
