using Microsoft.AspNetCore.SignalR;
using Schedent.BusinessLogic.Services;
using Schedent.Common.Enums;

namespace Schedent.API.Authorization
{
    public class UserProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            var token = connection.GetHttpContext().Request.Query["access_token"].ToString();

            if (token != null)
            {
                return JwtService.GetClaim(TokenClaim.UserId, token);
            }
            else
            {
                return null;
            }
        }
    }
}
