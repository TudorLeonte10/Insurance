using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Domain.Brokers
{
    public class Broker
    {
        public Guid Id { get; private set; }

        public string BrokerCode { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Phone { get; private set; } = string.Empty;

        public bool IsActive { get; private set; }

        public DateTime CreatedAt { get; private set; }

        private Broker() { }

        public static Broker Create(
            string brokerCode,
            string name,
            string email,
            string phone)
        {
            return new Broker
            {
                Id = Guid.NewGuid(),
                BrokerCode = brokerCode,
                Name = name,
                Email = email,
                Phone = phone,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
        }
    }

}
