using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Schedent.BusinessLogic.Services;
using System;
using System.Net;

namespace Schedent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacultyController : ControllerBase
    {
        private readonly FacultyService _facultyService;
        private readonly ILogger<FacultyController> _logger;

        public FacultyController(FacultyService facultyService, ILogger<FacultyController> logger)
        {
            _facultyService = facultyService;
            _logger = logger;
        }

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
                _logger.LogError(ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
