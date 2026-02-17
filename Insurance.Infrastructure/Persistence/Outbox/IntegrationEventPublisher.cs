using Insurance.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Insurance.Infrastructure.Persistence.Outbox
{
    public class IntegrationEventPublisher : IIntegrationEventPublisher
    {
        private readonly InsuranceDbContext _dbContext;
        public IntegrationEventPublisher(InsuranceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task Publish<T>(T integrationEvent, CancellationToken cancellationToken)
        {
            var outboxEvent = new OutboxEvent
            {
                Id = Guid.NewGuid(),
                EventType = typeof(T).Name,
                Payload = JsonSerializer.Serialize(integrationEvent),
                OccurredOn = DateTime.UtcNow,
                Processed = false
            };
           
            _dbContext.OutboxEvents.Add(outboxEvent);
            return Task.CompletedTask;
        }
    }
}
