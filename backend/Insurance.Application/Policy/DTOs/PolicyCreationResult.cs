using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.DTOs
{
    public class PolicyCreationResult
    {
        public Domain.Policies.Policy Policy { get; init; } = null!;

        public string Country { get; init; } = null!;
        public string County { get; init; } = null!;
        public string City { get; init; } = null!;

        public string BrokerCode { get; init; } = null!;
        public string Currency { get; init; } = null!;

        public string Status { get; init; } = null!;
        public string BuildingType { get; init; } = null!;

        public decimal FinalPremium { get; init; }
        public decimal FinalPremiumInBase { get; init; }

        public DateTime CreatedAt { get; init; }
    }
}
