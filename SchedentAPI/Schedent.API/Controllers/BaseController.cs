using Microsoft.AspNetCore.Mvc;
using Schedent.BusinessLogic.Services;
using Schedent.Common.Enums;
using System.Linq;

namespace Schedent.API.Controllers
{
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// Id of the current user
        /// </summary>
        /// <remarks>Do not use in public methods</remarks>
        private protected int? CurrentUserId
        {
            get
            {
                // Retrieve the token from the request headers
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token != null)
                {
                    // Get the user id from the token claims
                    return int.Parse(JwtService.GetClaim(TokenClaim.UserId, token));
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Id of the current user role
        /// </summary>
        private protected int? CurrentUserRoleId
        {
            get
            {
                // Retrieve the token from the request headers
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token != null)
                {
                    // Get the user role id from the token claims
                    return int.Parse(JwtService.GetClaim(TokenClaim.UserRoleId, token));
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
