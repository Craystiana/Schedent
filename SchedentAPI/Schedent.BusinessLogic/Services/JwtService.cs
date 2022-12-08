using Microsoft.IdentityModel.Tokens;
using Schedent.Common;
using Schedent.Common.Enums;
using Schedent.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Schedent.BusinessLogic.Services
{
    public static class JwtService
    {
        /// <summary>
        /// Create the userId and userRoleId claims and call the GetToken method with the list of claims
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(((int)TokenClaim.UserId).ToString(), user.UserId.ToString()),
                new Claim(((int)TokenClaim.UserRoleId).ToString(), user.UserRoleId.ToString()),
            };

            return GetToken(claims);
        }

        /// <summary>
        /// Retrieve the desired claim from the token
        /// </summary>
        /// <param name="claimKey"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string GetClaim(TokenClaim claimKey, string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenSecure = handler.ReadToken(token) as JwtSecurityToken;

            return tokenSecure.Claims.First(claim => claim.Type == ((int)claimKey).ToString()).Value;
        }

        /// <summary>
        /// Generate the token using the passed claims
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        private static string GetToken(IEnumerable<Claim> claims) 
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Settings.TokenSecretBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}