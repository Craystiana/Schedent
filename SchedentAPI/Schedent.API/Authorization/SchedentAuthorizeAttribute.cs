using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Schedent.Common;
using Schedent.Common.Enums;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Schedent.API.Authorization
{
    [AttributeUsage(AttributeTargets.All)]
    public class SchedentAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly int[] _userRoles;

        public SchedentAuthorizeAttribute()
        {
            _userRoles = new int[]
            {
               (int) UserRoleType.Student,
               (int) UserRoleType.Admin,
               (int) UserRoleType.Professor
            };
        }

        public SchedentAuthorizeAttribute(UserRoleType userRole)
        {
            _userRoles = new int[]
            {
                (int) userRole
            };
        }

        public SchedentAuthorizeAttribute(UserRoleType[] userRoles)
        {
            _userRoles = (int[])userRoles.Select(ur => (int)ur);
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token != null)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();

                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Settings.TokenSecretBytes),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;
                    _ = int.TryParse(jwtToken.Claims.FirstOrDefault(x => x.Type == ((int)TokenClaim.UserRoleId).ToString())?.Value, out var userRoleId);

                    if (!_userRoles.Contains(userRoleId))
                    {
                        context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                    }
                }
                else
                {
                    context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                }
            }
            catch
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }

        }
    }
}
