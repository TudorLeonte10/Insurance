
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Messaging.Rabbit
{
    [ExcludeFromCodeCoverage]
    public class RabbitMqPublisher : IAsyncDisposable
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;

        public RabbitMqPublisher(IConnection connection, IChannel channel)
        {
            _connection = connection;
            _channel = channel;
        }

        public static async Task<RabbitMqPublisher> CreateAsync(IConfiguration configuration, CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration["Rabbit:Host"] ?? "localhost"
            };
            var connection = await factory.CreateConnectionAsync(cancellationToken);
            var channel = await connection.CreateChannelAsync(null, cancellationToken);
            return new RabbitMqPublisher(connection, channel);
        }
        public async Task PublishAsync(
            string queueName,
            string message,
            string eventType,
            string? correlationId = null,
            CancellationToken cancellationToken = default)
        {

                await _channel.QueueDeclareAsync(
                    queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null,
                    cancellationToken: cancellationToken);

                var body = Encoding.UTF8.GetBytes(message ?? string.Empty);
                var properties = new BasicProperties
                {
                    Persistent = true,
                    Type = eventType
                };

                if (!string.IsNullOrEmpty(correlationId))
                    properties.CorrelationId = correlationId;

                try
                {
                    await _channel.BasicPublishAsync(
                        exchange: "",
                        routingKey: queueName,
                        mandatory: false,
                        basicProperties: properties,
                        body: body,
                        cancellationToken: cancellationToken);
                }
                catch (PublishException ex)
                {
                    throw new PublishRQException($"Failed to publish message to queue '{queueName}'", ex);
                }      
        }
        public async ValueTask DisposeAsync()
        {
            await _channel.DisposeAsync();
            await _connection.DisposeAsync();
        }
    }
}