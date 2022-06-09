using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Schedent.BusinessLogic.Services;
using System;
using System.Net;

namespace Schedent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : BaseController
    {
        private readonly ScheduleService _scheduleService;
        private readonly ILogger<ScheduleController> _logger;

        public ScheduleController(ScheduleService scheduleService, ILogger<ScheduleController> logger)
        {
            _scheduleService = scheduleService;
            _logger = logger;
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult Get()
        {
            try
            {
                return new JsonResult(_scheduleService.GetUserTimeTable((int)CurrentUserId, (int)CurrentUserRoleId));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("Subgroup")]
        public IActionResult GetSchedulesForSubgroup([FromQuery] int subgroupId)
        {
            try
            { 
                return new JsonResult(_scheduleService.GetSubgroupTimeTable(subgroupId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
