using Insurance.Infrastructure.Messaging.Rabbit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client.Exceptions;
using Insurance.Application.Abstractions.Loggers;
using System.Diagnostics.CodeAnalysis;

namespace Insurance.Infrastructure.Persistence.Outbox
{
    [ExcludeFromCodeCoverage]
    public class OutboxPublisherBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;

        public OutboxPublisherBackgroundService(
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<InsuranceDbContext>();
                var publisher = scope.ServiceProvider.GetRequiredService<RabbitMqPublisher>();

                var logger = scope.ServiceProvider.GetRequiredService<IApplicationLogger>();

                var events = await db.OutboxEvents
                    .Where(x => !x.Processed && !x.Enqueued)
                    .OrderBy(x => x.OccurredOn)
                    .Take(20)
                    .ToListAsync(cancellationToken);

                foreach (var ev in events)
                {
                    try
                    {
                        await publisher.PublishAsync(
                            _configuration["Rabbit:Queue"]!,
                            ev.Payload,
                            ev.EventType,
                            correlationId: ev.Id.ToString(),
                            cancellationToken);

                        ev.Enqueued = true;
                        await db.SaveChangesAsync(cancellationToken);
                    }
                    catch (PublishRQException ex)
                    {
                        logger.LogError(ex, "Failed to publish outbox event {EventId} of type {EventType}", ev.Id, ev.EventType);
                    }

                    await Task.Delay(5000, cancellationToken);
                }
            }
        }
    }
}

