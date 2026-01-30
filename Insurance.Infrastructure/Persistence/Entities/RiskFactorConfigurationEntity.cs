using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Entities
{
    public class RiskFactorConfigurationEntity
    {
        public Guid Id { get; set; }

        public RiskFactorLevel Level { get; set; }

        public string ReferenceId { get; set; } = null!;

        public decimal AdjustmentPercentage { get; set; }

        public bool IsActive { get; set; }
    }

}
