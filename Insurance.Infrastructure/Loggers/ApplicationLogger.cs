using Insurance.Application.Abstractions.Loggers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Loggers
{
    public class ApplicationLogger : IApplicationLogger
    {
        private readonly ILogger<ApplicationLogger> _logger;
        public ApplicationLogger(ILogger<ApplicationLogger> logger)
        {
            _logger = logger;
        }
        public void LogError(Exception ex, string message)
        {
            _logger.LogError(ex, message);
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }
    }
}
