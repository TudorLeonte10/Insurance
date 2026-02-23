using Insurance.Application.Events;
using Insurance.Infrastructure.Messaging.Rabbit;
using Insurance.Infrastructure.Persistence;
using Insurance.Reporting.Infrastructure.Entities;
using Insurance.Reporting.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace Insurance.Reporting.Worker.Consumer
{
    public class PolicyCreatedConsumerBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly RabbitMqPublisher _publisher;
        private IConnection? _connection;
        private IChannel? _channel;

        public PolicyCreatedConsumerBackgroundService(IServiceScopeFactory scopeFactory, IConfiguration configuration, RabbitMqPublisher publisher)
        {
            _publisher = publisher;
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["Rabbit:Host"] ?? "localhost"
            };

            try
            {
                _connection = await factory.CreateConnectionAsync(cancellationToken);
                _channel = await _connection.CreateChannelAsync(null, cancellationToken);

                await _channel.QueueDeclareAsync(
                    queue: _configuration["Rabbit:Queue"]!,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: new Dictionary<string, object?>
                    {
                        {"x-delivery-limit", 5 }
                    },
                    cancellationToken: cancellationToken);

            }
            catch (BrokerUnreachableException ex)
            {
                Console.WriteLine("Exception at RabbitMQ: " + ex.ToString());
            }

            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel!);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                try
                {
                    var correlationId = ea.BasicProperties.CorrelationId;
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var eventType = ea.BasicProperties.Type;

                    using var scope = _scopeFactory.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<IPolicyIntegrationEventHandler>();

                    var processed = await handler.Handle(eventType, body, stoppingToken);

                    if(processed && !string.IsNullOrEmpty(correlationId))
                    {
                        var ackQueue = _configuration["Rabbit:OutboxAckQueue"] ?? "ack_queue";
                        await _publisher.PublishAsync(ackQueue, string.Empty, "OutboxAck", correlationId, stoppingToken);
                    }

                    await _channel!.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                }
                catch (RabbitMQClientException)
                {
                    await _channel!.BasicNackAsync(ea.DeliveryTag, false, true, stoppingToken);
                }
            };

            await _channel!.BasicConsumeAsync(
                queue: _configuration["Rabbit:Queue"]!,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);
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
