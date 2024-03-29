﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Schedent.BusinessLogic.Services;
using System;
using System.Net;

namespace Schedent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : BaseController
    {
        private readonly SectionService _sectionService;
        private readonly ILogger<SectionController> _logger;

        /// <summary>
        /// SectionController constructor
        /// Inject the SectionService and the logger
        /// </summary>
        /// <param name="sectionService"></param>
        /// <param name="logger"></param>
        public SectionController(SectionService sectionService, ILogger<SectionController> logger)
        {
            _sectionService = sectionService;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint for retrieving all the sections of the given faculty
        /// </summary>
        /// <param name="facultyId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("All")]
        public IActionResult All([FromQuery] int facultyId)
        {
            try
            {
                return new JsonResult(_sectionService.GetListOfSections(facultyId));
            }
            catch(Exception ex)
            {
                _logger.LogError("An error occured: {error}", ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
