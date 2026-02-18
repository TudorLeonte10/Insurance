using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Events
{
    public record PolicyStatusChangedIntegrationEvent(
        Guid PolicyId,
        string NewStatus,
        DateTime ChangedAt);
    
    
}
