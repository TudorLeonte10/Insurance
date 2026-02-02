using Insurance.Domain.Metadata.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Domain.Metadata
{
    public class RiskFactorConfiguration
    {
        public Guid Id { get; set; }

        public RiskFactorLevel Level { get; set; }

        public string ReferenceId { get; set; } = string.Empty;

        public decimal AdjustmentPercentage { get; set; }

        public bool IsActive { get; set; }
    }
}
