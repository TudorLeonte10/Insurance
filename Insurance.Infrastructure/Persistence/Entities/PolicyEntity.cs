using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Entities
{
    public class PolicyEntity
    {
        public Guid Id { get; set; }

        public string PolicyNumber { get; set; } = null!;

        public PolicyStatus Status { get; set; }

        public Guid ClientId { get; set; }
        public ClientEntity Client { get; set; } = null!;

        public Guid BuildingId { get; set; }
        public BuildingEntity Building { get; set; } = null!;

        public Guid BrokerId { get; set; }
        public BrokerEntity Broker { get; set; } = null!;

        public Guid CurrencyId { get; set; }
        public CurrencyEntity Currency { get; set; } = null!;

        public decimal BasePremium { get; set; }
        public decimal FinalPremium { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ActivatedAt { get; set; }
        public DateTime? CancelledAt { get; set; }

        public string? CancellationReason { get; set; }
    }

}
