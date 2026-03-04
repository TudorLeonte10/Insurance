using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Insurance.Tests.Integration.Setup
{
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string SchemeName = "Test";

        public TestAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            var headers = Request.Headers;

            var userIdString = headers.TryGetValue("X-Test-UserId", out var uid) ? uid.ToString() : Guid.NewGuid().ToString();
            var name = headers.TryGetValue("X-Test-UserName", out var uname) ? uname.ToString() : "test-user";

            var rolesHeader = headers.TryGetValue("X-Test-Roles", out var roles) ? roles.ToString() : "Admin";

            var brokerIdString = headers.TryGetValue("X-Test-BrokerId", out var bid) ? bid.ToString() : Guid.NewGuid().ToString();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userIdString),
                new Claim(ClaimTypes.Name, name),
                new Claim("brokerId", brokerIdString)
            };

            var identity = new ClaimsIdentity(claims, SchemeName);

            if (!string.IsNullOrWhiteSpace(rolesHeader))
            {
                foreach (var role in rolesHeader.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role.Trim()));
                }
            }

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, SchemeName);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
