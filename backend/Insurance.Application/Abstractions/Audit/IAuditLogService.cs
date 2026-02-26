using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Abstractions.Audit
{
    public interface IAuditLogService
    {
        Task LogAsync(AuditEntry entry, CancellationToken cancellationToken);
    }
}
