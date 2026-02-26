using Insurance.Domain.Metadata.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Metadata.RiskFactors.DTOs
{
    public class CreateRiskFactorConfigurationDto
    {
        public RiskFactorLevel Level { get; init; }
        public string ReferenceId { get; init; } = string.Empty;
        public decimal AdjustmentPercentage { get; init; }
    }

}
