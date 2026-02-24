using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Abstractions.Messaging
{
    public interface IIntegrationEventPublisher
    {
        Task Publish<T>(T integrationEvent, CancellationToken cancellationToken);
    }
}
