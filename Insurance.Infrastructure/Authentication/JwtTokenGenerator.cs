using Insurance.Application.Authentication;
using Insurance.Infrastructure.Persistence.Entities;
using Insurance.Infrastructure.Persistence.Migrations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Insurance.Infrastructure.Authentication
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _settings;

        public JwtTokenGenerator(IOptions<JwtSettings> options)
        {
            _settings = options.Value;
        }

        public string Generate(AuthUserContext userContext)
        {
            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userContext.UserId.ToString()),
            new(ClaimTypes.Name, userContext.Username),
            new(ClaimTypes.Role, userContext.Role.ToString())
        };

            if (userContext.BrokerId.HasValue)
            {
                claims.Add(new Claim(
                    "brokerId",
                    userContext.BrokerId.Value.ToString()));
            }

            if (userContext.Role == "Admin")
            {
                claims.Add(new Claim("isAdmin", "true"));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_settings.Key));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_settings.ExpiresMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
