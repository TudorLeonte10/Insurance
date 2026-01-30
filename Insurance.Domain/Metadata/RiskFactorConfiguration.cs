using Insurance.Domain.Metadata.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Domain.Metadata
{
    public class RiskFactorConfiguration
    {
        public Guid Id { get; private set; }

        public RiskFactorLevel Level { get; private set; }

        public string ReferenceId { get; private set; } = string.Empty;

        public decimal AdjustmentPercentage { get; private set; }

        public bool IsActive { get; private set; }
    }
}
