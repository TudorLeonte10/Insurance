using Insurance.Domain.Buildings;
using Insurance.Domain.Policies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.DTOs
{
    public class PolicyReportReadModel
    {
        public Guid PolicyId { get; set; }
        public DateTime PolicyStartDate { get; set; }

        public string Country { get; set; } = string.Empty;
        public string County { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public Guid CityId { get; set; }

        public string BrokerCode { get; set; } = string.Empty;
        public string BrokerName { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = string.Empty;

        public decimal FinalPremium { get; set; }
        public decimal ExchangeRate { get; set; }

        public PolicyStatus Status { get; set; }
        public BuildingType BuildingType { get; set; }
    }

}
