﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Schedent.BusinessLogic.Services;
using System;
using System.Net;

namespace Schedent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : BaseController
    {
        private readonly GroupService _groupService;
        private readonly ILogger<GroupController> _logger;

        /// <summary>
        /// GroupController constructor
        /// Inject the GroupService and the logger
        /// </summary>
        /// <param name="groupService"></param>
        /// <param name="logger"></param>
        public GroupController(GroupService groupService, ILogger<GroupController> logger)
        {
            _groupService = groupService;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint for retrieving all groups based on the given section
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("All")]
        public IActionResult All([FromQuery] int sectionId)
        {
            try
            {
                return new JsonResult(_groupService.GetListOfGroups(sectionId));
            }
            catch(Exception ex)
            {
                _logger.LogError("An error occured: {error}", ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
