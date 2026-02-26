using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Abstractions.Loggers
{
    public interface IAuditLogger 
    {
        void LogAudit(string action, string entity, Guid entityId);
    }
}
