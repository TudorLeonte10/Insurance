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
                context.Items["CorrelationId"] = correlationId;
            }

            context.Response.OnStarting(() =>
            {
                if (!context.Response.Headers.ContainsKey("CorrelationId"))
                {
                    context.Response.Headers.Add("CorrelationId", context.Items["CorrelationId"].ToString());
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
                ["CorrelationId"] = context.Items["CorrelationId"].ToString(),
                ["UserId"] = userId,
                ["BrokerId"] = brokerId != Guid.Empty ? brokerId.ToString() : "N/A"
            }))
            {
                await _next(context);
            }

        }
    }
}
