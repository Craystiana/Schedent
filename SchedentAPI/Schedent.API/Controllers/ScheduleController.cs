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

        /// <summary>
        /// ScheduleController constructor
        /// Inject the ScheduleService and the logger
        /// </summary>
        /// <param name="scheduleService"></param>
        /// <param name="logger"></param>
        public ScheduleController(ScheduleService scheduleService, ILogger<ScheduleController> logger)
        {
            _scheduleService = scheduleService;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint for retrieving the schedules of the logged in user
        /// </summary>
        /// <returns></returns>
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
                _logger.LogError("An error occured: {error}", ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Endpoint for retrieving the schedules for the given subgroup
        /// </summary>
        /// <param name="subgroupId"></param>
        /// <returns></returns>
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
                _logger.LogError("An error occured: {error}", ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
