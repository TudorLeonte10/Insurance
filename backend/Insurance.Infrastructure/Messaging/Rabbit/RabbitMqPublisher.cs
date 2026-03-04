
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
        private readonly IConfiguration _configuration;
        private IConnection? _connection;
        private IChannel? _channel;

        public RabbitMqPublisher(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private async Task EnsureConnectionAsync(CancellationToken cancellationToken)
        {
            if (_connection != null && _connection.IsOpen)
                return;

            var factory = new ConnectionFactory
            {
                HostName = _configuration["Rabbit:Host"] ?? "localhost"
            };

            _connection = await factory.CreateConnectionAsync(cancellationToken);
            _channel = await _connection.CreateChannelAsync(null, cancellationToken);
        }

        public async Task PublishAsync(
            string queueName,
            string message,
            string eventType,
            string? correlationId = null,
            CancellationToken cancellationToken = default)
        {
            await EnsureConnectionAsync(cancellationToken);

            await _channel!.QueueDeclareAsync(
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

            await _channel.BasicPublishAsync(
                exchange: "",
                routingKey: queueName,
                mandatory: false,
                basicProperties: properties,
                body: body,
                cancellationToken: cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel != null)
                await _channel.DisposeAsync();

            if (_connection != null)
                await _connection.DisposeAsync();
        }
    }
}