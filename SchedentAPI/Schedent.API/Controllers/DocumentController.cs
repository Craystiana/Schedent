using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Schedent.API.Authorization;
using Schedent.BusinessLogic.Services;
using Schedent.Common.Enums;
using Schedent.Domain.DTO.Document;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Schedent.API.Hubs;

namespace Schedent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : BaseController
    {
        private readonly DocumentService _documentService;
        private readonly ILogger<DocumentController> _logger;
        private readonly IHubContext<NotificationHub> _notificationHub;

        public DocumentController(DocumentService documentService, ILogger<DocumentController> logger, IHubContext<NotificationHub> notificationHub)
        {
            _documentService = documentService;
            _logger = logger;
            _notificationHub = notificationHub;
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
                _logger.LogError("An error occured: {error}", ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<ActionResult> Upload()
        {
            try
            {
                await _notificationHub.Clients.User("13").SendAsync("ReceiveOne", "New schedule!");

                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError("An error occured: {error}", ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
