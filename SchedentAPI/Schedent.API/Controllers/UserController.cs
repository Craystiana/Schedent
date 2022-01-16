using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Schedent.BusinessLogic.Services;
using Schedent.Domain.DTO.User;
using System;
using System.Net;

namespace Schedent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly UserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(UserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            try
            {
                var user = _userService.Login(model.Email, model.Password);

                if (user != null)
                {
                    return Ok(new LoginResponseModel
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        EmailAddress = user.Email,
                        UserRole = user.UserRoleId,
                        Token = JwtService.GenerateToken(user)
                    });
                }

                return Forbid();
            }
            catch
            {
                return Forbid();
            }
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            try
            {
                var result = _userService.Register(model);

                return new JsonResult(result != null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while registering user ");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
