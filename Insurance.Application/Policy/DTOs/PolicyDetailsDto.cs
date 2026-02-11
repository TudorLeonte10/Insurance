using Insurance.Application.Buildings.DTOs;
using Insurance.Application.Clients.DTOs;
using Insurance.Application.Metadata.Currency.DTOs;
using Insurance.Domain.Policies;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Application.Policy.DTOs
{
    [ExcludeFromCodeCoverage]
    public class PolicyDetailsDto
    {
        public Guid Id { get; set; }
        public string PolicyNumber { get; set; } = string.Empty;
        public PolicyStatus Status { get; set; }

        public ClientSummaryDto Client { get; set; } = default!;
        public BuildingSummaryDto Building { get; set; } = default!;
        public CurrencyDto Currency { get; set; } = default!;

        public decimal BasePremium { get; set; }
        public decimal FinalPremium { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }


}
