using Insurance.Domain.Metadata.Enums;
using Insurance.Domain.RiskIndicators;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace Insurance.Domain.Metadata
{
    public class FeeConfiguration
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public FeeType Type { get; set; }

        public RiskIndicatorType? RiskIndicatorType { get; set; }

        public decimal Percentage { get; set; }

        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
