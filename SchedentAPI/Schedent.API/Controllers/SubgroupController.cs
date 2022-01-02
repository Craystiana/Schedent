using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Schedent.BusinessLogic.Services;
using System;
using System.Net;

namespace Schedent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubgroupController : ControllerBase
    {
        private readonly SubgroupService _subgroupService;
        private readonly ILogger<SubgroupController> _logger;

        public SubgroupController(SubgroupService subgroupService, ILogger<SubgroupController> logger)
        {
            _subgroupService = subgroupService;
            _logger = logger;
        }

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
                _logger.LogError(ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
