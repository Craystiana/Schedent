using Microsoft.AspNetCore.Mvc;
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

        public SectionController(SectionService sectionService, ILogger<SectionController> logger)
        {
            _sectionService = sectionService;
            _logger = logger;
        }

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
                _logger.LogError(ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
