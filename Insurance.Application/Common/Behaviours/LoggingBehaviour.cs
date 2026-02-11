using Insurance.Application.Abstractions.Loggers;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Application.Common.Behaviours
{
    [ExcludeFromCodeCoverage]
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IApplicationLogger _logger;

        public LoggingBehaviour(IApplicationLogger logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation($"Handling {requestName} {request}");

            try
            {
                var response = await next();

                stopwatch.Stop();

                _logger.LogInformation($"Handled {requestName} in {stopwatch.ElapsedMilliseconds} ms");

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.LogError(ex,
                    $"Error handling {requestName} after {stopwatch.ElapsedMilliseconds} ms. Exception type: {ex.GetType().Name}");
                throw;
            }
        }
    }
}
