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
        public static string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new Claim(((int)TokenClaim.UserId).ToString(), user.UserId.ToString()),
                new Claim(((int)TokenClaim.UserRoleId).ToString(), user.UserRoleId.ToString()),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Settings.TokenSecretBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public static string GetClaim(TokenClaim claimKey, string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenSecure = handler.ReadToken(token) as JwtSecurityToken;

            return tokenSecure.Claims.First(claim => claim.Type == ((int)claimKey).ToString()).Value;
        }
    }
}