using Insurance.Infrastructure.Messaging.Rabbit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Outbox
{
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

                var events = await db.OutboxEvents
                    .Where(x => !x.Processed)
                    .OrderBy(x => x.OccurredOn)
                    .Take(20)
                    .ToListAsync(cancellationToken);

                foreach (var @event in events)
                {
                    await publisher.PublishAsync(
                        _configuration["Rabbit:Queue"]!,
                        @event.Payload,
                        cancellationToken);

                    @event.Processed = true;
                }

                await db.SaveChangesAsync(cancellationToken);

                await Task.Delay(2000, cancellationToken);
            }
        }
    }
}

