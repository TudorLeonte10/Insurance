using Insurance.Domain.Metadata.Enums;
using Insurance.Domain.RiskIndicators;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.AccessControl;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Entities
{
    [ExcludeFromCodeCoverage]
    public class FeeConfigurationEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public FeeType Type { get; set; }

        public RiskIndicatorType? RiskIndicatorType { get; set; }

        public decimal Percentage { get; set; }

        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public bool IsActive { get; set; }
    }


}
