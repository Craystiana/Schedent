using Microsoft.AspNetCore.SignalR;
using Schedent.BusinessLogic.Services;
using Schedent.Common.Enums;

namespace Schedent.API.Authorization
{
    public class UserProvider : IUserIdProvider
    {
        // Method used for retieving the user id from the token
        public string GetUserId(HubConnectionContext connection)
        {
            // Get the token from the current request
            var token = connection.GetHttpContext().Request.Query["access_token"].ToString();

            // In case the token is not null try to get the UserId claim
            // Otherwise return null
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
