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
    }

}
