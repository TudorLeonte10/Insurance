using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Abstractions.Audit
{
    public class AuditEntry
    {
        public string EntityType { get; init; } = string.Empty;
        public Guid EntityId { get; init; }
        public DateTime ChangedAt { get; init; }
        public string ChangedBy { get; init; } = string.Empty;

        public IReadOnlyCollection<AuditChangeEntry> Changes { get; init; }
            = Array.Empty<AuditChangeEntry>();
    }

    public class AuditChangeEntry
    {
        public string Field { get; init; } = string.Empty;
        public string OldValue { get; init; }
        public string NewValue { get; init; }
    }
}
