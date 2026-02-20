using Insurance.Application.Events;
using Insurance.Infrastructure.Messaging.Rabbit;
using Insurance.Infrastructure.Persistence;
using Insurance.Reporting.Infrastructure.Entities;
using Insurance.Reporting.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;

namespace Insurance.Reporting.Worker.Consumer
{
    [ExcludeFromCodeCoverage]
    public class PolicyCreatedConsumerBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private IConnection? _connection;
        private IChannel? _channel;

        public PolicyCreatedConsumerBackgroundService(IServiceScopeFactory scopeFactory, IConfiguration configuration, RabbitMqPublisher publisher)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["Rabbit:Host"] ?? "localhost"
            };

            _connection = await factory.CreateConnectionAsync(cancellationToken);
            _channel = await _connection.CreateChannelAsync(null, cancellationToken);

            await _channel.QueueDeclareAsync(
                queue: _configuration["Rabbit:Queue"]!,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken);

            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel!);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var correlationId = ea.BasicProperties.CorrelationId;


                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var eventType = ea.BasicProperties.Type;

                    using var scope = _scopeFactory.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<ReportingDbContext>();

                    var processedSuccessfully = false;
                    
                Console.WriteLine("Event Type: " + eventType);

                switch (eventType)
                    {
                        case nameof(PolicyCreatedIntegrationEvent):
                            var createdEvent = JsonSerializer.Deserialize<PolicyCreatedIntegrationEvent>(message);

                            var aggregate = new PolicyReportAggregate
                            {
                                PolicyId = createdEvent!.PolicyId,
                                Country = createdEvent.Country,
                                County = createdEvent.County,
                                City = createdEvent.City,
                                BrokerCode = createdEvent.BrokerCode,
                                Currency = createdEvent.Currency,
                                Status = createdEvent.Status,
                                BuildingType = createdEvent.BuildingType,
                                FinalPremium = createdEvent.FinalPremium,
                                FinalPremiumInBase = createdEvent.FinalPremiumInBase,
                                CreatedAt = DateTime.UtcNow
                            };

                            if (!await db.PolicyReportAggregates
                                .AnyAsync(x => x.PolicyId == createdEvent!.PolicyId, stoppingToken))
                            {
                                db.PolicyReportAggregates.Add(aggregate);
                                await db.SaveChangesAsync(stoppingToken);
                            }

                            processedSuccessfully = true;
                        break;

                        case nameof(PolicyStatusChangedIntegrationEvent):
                            var statusChangedEvent = JsonSerializer.Deserialize<PolicyStatusChangedIntegrationEvent>(message);

                            var aggregateToUpdate = await db.PolicyReportAggregates
                                    .FirstOrDefaultAsync(x => x.PolicyId == statusChangedEvent!.PolicyId, stoppingToken);

                            if (aggregateToUpdate != null)
                            {
                                aggregateToUpdate.Status = statusChangedEvent!.NewStatus;
                                await db.SaveChangesAsync(stoppingToken);
                            }

                            processedSuccessfully = true;
                        break;
                    }

                if (processedSuccessfully && !string.IsNullOrEmpty(correlationId) && Guid.TryParse(correlationId, out var outboxId))
                {
                    var primaryDb = scope.ServiceProvider.GetService<InsuranceDbContext>();
                    if (primaryDb != null)
                    {
                        var outbox = await primaryDb.OutboxEvents.FirstOrDefaultAsync(o => o.Id == outboxId, stoppingToken);
                        if (outbox != null && outbox.Enqueued && !outbox.Processed)
                        {
                            outbox.Processed = true;
                            await primaryDb.SaveChangesAsync(stoppingToken);
                        }
                    }
                }

                await _channel!.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            };

            await _channel!.BasicConsumeAsync(queue: _configuration["Rabbit:Queue"]!, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel != null)
                await _channel.CloseAsync(cancellationToken);

            if (_connection != null)
                await _connection.CloseAsync(cancellationToken);

            await base.StopAsync(cancellationToken);
        }
    }
}
