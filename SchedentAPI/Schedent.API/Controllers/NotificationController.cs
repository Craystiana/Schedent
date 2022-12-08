using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Schedent.BusinessLogic.Services;
using System;
using System.Net;

namespace Schedent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : BaseController
    {
        private readonly NotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        /// <summary>
        /// NotificationController constructor
        /// Injejct the NotificationService and the logger
        /// </summary>
        /// <param name="notificationService"></param>
        /// <param name="logger"></param>
        public NotificationController(NotificationService notificationService, ILogger<NotificationController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint for retrieving all the notifications of the logged in user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("All")]
        public IActionResult All()
        {
            try
            {
                return new JsonResult(_notificationService.GetUserNotification((int)CurrentUserId));
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured: {error}", ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
