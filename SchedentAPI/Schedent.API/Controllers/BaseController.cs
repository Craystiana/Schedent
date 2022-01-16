using Microsoft.AspNetCore.Mvc;
using Schedent.BusinessLogic.Services;
using Schedent.Common.Enums;
using System;
using System.Linq;

namespace Schedent.API.Controllers
{
    public class BaseController : ControllerBase
    {
        public BaseController() { }

        /// <summary>
        /// Id of the current user
        /// </summary>
        /// <remarks>Do not use in public methods</remarks>
        private protected int? CurrentUserId
        {
            get
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token != null)
                {
                    return int.Parse(JwtService.GetClaim(TokenClaim.UserId, token));
                }
                else
                {
                    return null;
                }
            }
        }

        private protected int? CurrentUserRoleId
        {
            get
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token != null)
                {
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
