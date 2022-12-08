using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Schedent.BusinessLogic.Services;
using System;
using System.Net;

namespace Schedent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacultyController : BaseController
    {
        private readonly FacultyService _facultyService;
        private readonly ILogger<FacultyController> _logger;

        /// <summary>
        /// FacultyController constructor
        /// Inject the FacultyService and the logger
        /// </summary>
        /// <param name="facultyService"></param>
        /// <param name="logger"></param>
        public FacultyController(FacultyService facultyService, ILogger<FacultyController> logger)
        {
            _facultyService = facultyService;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint for retrieving the list of all faculties present in the database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("All")]
        public IActionResult All()
        {
            try
            {
                return new JsonResult(_facultyService.GetListOfFaculties());
            }
            catch(Exception ex)
            {
                _logger.LogError("An error occured: {error}", ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
