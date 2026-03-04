using DnsClient.Internal;
using Insurance.Application.Abstractions.Loggers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Loggers
{
    [ExcludeFromCodeCoverage]
    public class AuditLogger : IAuditLogger
    {
        private readonly ILogger<AuditLogger> _logger;
        public AuditLogger(ILogger<AuditLogger> logger)
        {
            _logger = logger;
        }
        public void LogAudit(string action, string entity, Guid entityId)
        {
            _logger.LogInformation(
                "Audit event. Action={Action}, Entity={Entity}, EntityId={EntityId}",
                action,
                entity,
                entityId);
        }
    }
}
