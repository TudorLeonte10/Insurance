using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Reporting.Infrastructure.Entities
{
    public class PolicyReportAggregate
    {
        public Guid PolicyId { get; set; }

        public string Country { get; set; } = null!;
        public string County { get; set; } = null!;
        public string City { get; set; } = null!;

        public string BrokerCode { get; set; } = null!;

        public string Currency { get; set; } = null!;

        public string Status { get; set; } = null!;
        public string BuildingType { get; set; } = null!;

        public decimal FinalPremium { get; set; }
        public decimal FinalPremiumInBase { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
