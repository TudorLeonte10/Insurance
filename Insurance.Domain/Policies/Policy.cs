using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Domain.Policies
{
    public class Policy
    {
        public Guid Id { get; private set; }

        public string PolicyNumber { get; private set; } = string.Empty;

        public PolicyStatus Status { get; private set; }

        public Guid ClientId { get; private set; }
        public Guid BuildingId { get; private set; }
        public Guid BrokerId { get; private set; }
        public Guid CurrencyId { get; private set; }

        public decimal BasePremium { get; private set; }
        public decimal FinalPremium { get; private set; }

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime? ActivatedAt { get; private set; }
        public DateTime? CancelledAt { get; private set; }

        public string? CancellationReason { get; private set; }

    }

}
