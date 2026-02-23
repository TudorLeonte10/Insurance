using Insurance.Application.Authentication;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Insurance.WebApi.Middleware
{
    public class TraceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TraceMiddleware> _logger;

        public TraceMiddleware(RequestDelegate next, ILogger<TraceMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
           

            if (context.Request.Headers.ContainsKey("xCorrelationId"))
            {
                context.Items["xCorrelationId"] = context.Request.Headers["xCorrelationId"].ToString();
            }
            else
            {
                var correlationId = Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString();
                context.Items["xCorrelationId"] = correlationId;
            }

            context.Response.OnStarting(() =>
            {
                if (!context.Response.Headers.ContainsKey("xCorrelationId"))
                {
                    context.Response.Headers.Add("xCorrelationId", context.Items["xCorrelationId"].ToString());
                }
                return Task.CompletedTask;
            });

            var userId = Guid.Empty;
            var brokerId = Guid.Empty;
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var currentUserContext = context.RequestServices.GetRequiredService<ICurrentUserContext>();      
                userId = currentUserContext.UserId;
                brokerId = currentUserContext.BrokerId ?? Guid.Empty;
            }

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["xCorrelationId"] = context.Items["xCorrelationId"].ToString(),
                ["UserId"] = userId,
                ["BrokerId"] = brokerId != Guid.Empty ? brokerId.ToString() : "N/A"
            }))
            {
                await _next(context);
            }

        }
    }
}
