using Insurance.Reporting.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Reporting.Worker.Consumer
{
    public interface IPolicyIntegrationEventHandler
    {
        Task<bool> Handle(string eventType, string message, CancellationToken cancellationToken);
    }
}
