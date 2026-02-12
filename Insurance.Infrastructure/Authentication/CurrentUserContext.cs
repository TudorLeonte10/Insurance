using Insurance.Application.Authentication;
using Insurance.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Insurance.Infrastructure.Authentication
{
    public class CurrentUserContext : ICurrentUserContext
    {
        public Guid UserId { get; }
        public string Role { get; } = string.Empty;
        public Guid? BrokerId { get; }

        public CurrentUserContext(IHttpContextAccessor httpContext)
        {
            var user = httpContext.HttpContext?.User
                ?? throw new UnauthorizedException("Not authenticated");

            UserId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            Role = user.FindFirst(ClaimTypes.Role)!.Value;
            
            var brokerIdClaim = user.FindFirst("brokerId")?.Value;

            if (!string.IsNullOrEmpty(brokerIdClaim) && Guid.TryParse(brokerIdClaim, out var brokerId))
            {
                BrokerId = brokerId;
            }
        }
    }
}
