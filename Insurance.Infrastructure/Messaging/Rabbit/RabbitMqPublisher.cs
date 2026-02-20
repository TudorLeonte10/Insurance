
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
    public class RabbitMqPublisher
    {
        private readonly Task<IConnection> _connectionTask;

        public RabbitMqPublisher(IConfiguration configuration)
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration["Rabbit:Host"] ?? "localhost"
            };

            _connectionTask = factory.CreateConnectionAsync();
        }

        public async Task PublishAsync(
            string queueName,
            string message,
            string eventType,
            string? correlationId = null,
            CancellationToken cancellationToken = default)
        {
            var connection = await _connectionTask;

            await using var channel = await connection.CreateChannelAsync(null, cancellationToken);

            await channel.QueueDeclareAsync(
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
                await channel.BasicPublishAsync(
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
    }
}