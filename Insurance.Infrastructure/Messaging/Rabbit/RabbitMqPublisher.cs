
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Messaging.Rabbit
{
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

            var body = Encoding.UTF8.GetBytes(message);

            var properties = new BasicProperties
            {
                Persistent = true,
                Type = eventType
            };

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: queueName,
                mandatory: false,
                basicProperties: properties,
                body: body,
                cancellationToken: cancellationToken);
        }
    }
}