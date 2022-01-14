using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Schedent.API.Authorization;
using Schedent.BusinessLogic.Services;
using Schedent.Common.Enums;
using Schedent.Domain.DTO.Document;
using System;
using System.Net;

namespace Schedent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly DocumentService _documentService;
        private readonly ILogger<DocumentController> _logger;

        public DocumentController(DocumentService documentService, ILogger<DocumentController> logger)
        {
            _documentService = documentService;
            _logger = logger;
        }

        [HttpPost]
        [Route("Add")]
        [SchedentAuthorize(UserRoleType.Admin)]
        public IActionResult Add([FromBody] DocumentModel file)
        {
            try
            {
                var result = _documentService.Add(file.File);

                return new JsonResult(result != null);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
