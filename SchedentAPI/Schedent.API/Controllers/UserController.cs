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

        /// <summary>
        /// UserController constructor
        /// Inject the UserService and the logger
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="logger"></param>
        public UserController(UserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint for authentication based on the email and the password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Endpoint for creating a new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Endpoint for retrieving the logged in user details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Details")]
        public IActionResult GetUserDetails()
        {
            try
            {
                return new JsonResult(_userService.GetUserDetails((int)CurrentUserId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting the user details");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Endpoint for updating the profile of the logged in user
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Profile")]
        public IActionResult Profile([FromBody] UserDetails profile)
        {
            try
            {
                _userService.EditProfile(profile, (int)CurrentUserId);

                return new JsonResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting the user details");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Endpoint for updating the device token of the logged in user
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("DeviceToken/{token}")]
        public IActionResult DeviceToken(string token)
        {
            try
            {
                _userService.EditDeviceToken((int)CurrentUserId, token);

                return new JsonResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while editing the user device token");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
