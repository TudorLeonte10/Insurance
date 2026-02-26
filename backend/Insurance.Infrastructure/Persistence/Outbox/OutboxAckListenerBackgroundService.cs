using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Outbox
{
    public class OutboxAckListenerBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private IConnection? _connection;
        private IChannel? _channel;

        public OutboxAckListenerBackgroundService(IServiceScopeFactory scopeFactory, IConfiguration configuration)
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

                try {

                    _connection = await factory.CreateConnectionAsync(cancellationToken);
                    _channel = await _connection.CreateChannelAsync(null, cancellationToken);

                    var ackQueue = _configuration["Rabbit:OutboxAckQueue"] ?? "outbox-acks";

                    await _channel.QueueDeclareAsync(
                        queue: ackQueue,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null,
                        cancellationToken: cancellationToken);

                }
                catch(BrokerUnreachableException ex)
                {
                    Console.WriteLine("RabbitMQ not available: " + ex.ToString());
                    throw;
                }

            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var ackQueue = _configuration["Rabbit:OutboxAckQueue"] ?? "outbox-acks";
            var consumer = new AsyncEventingBasicConsumer(_channel!);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var correlationId = ea.BasicProperties?.CorrelationId;
                if (!string.IsNullOrEmpty(correlationId) && Guid.TryParse(correlationId, out var outboxId))
                {
                    using var scope = _scopeFactory.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<InsuranceDbContext>();

                    var outbox = await db.OutboxEvents.FirstOrDefaultAsync(o => o.Id == outboxId, stoppingToken);
                    if (outbox != null && outbox.Enqueued && !outbox.Processed)
                    {
                        outbox.Processed = true;
                        await db.SaveChangesAsync(stoppingToken);
                    }
                }
                if(_channel is not null)
                    await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            };

            if(_channel is not null)
                await _channel.BasicConsumeAsync(queue: ackQueue, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel is not null)
                await _channel.CloseAsync(cancellationToken);
            if (_connection is not null)
                await _connection.CloseAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
        }
    }
}
