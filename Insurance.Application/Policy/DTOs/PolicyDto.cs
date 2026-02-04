using Insurance.Domain.Policies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.DTOs
{
    public class PolicyDetailsDto
    {
        public Guid Id { get; set; }
        public string PolicyNumber { get; set; } = string.Empty;

        public PolicyStatus Status { get; set; }

        public Guid ClientId { get; set; }
        public Guid BuildingId { get; set; }
        public Guid BrokerId { get; set; }
        public Guid CurrencyId { get; set; }

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
