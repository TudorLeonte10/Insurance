using Insurance.Application.Policy.Services;
using Insurance.Domain.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.RiskStrategies
{
    public interface IRiskFactorCalculator
    {
        decimal ApplyRiskFactors(
            decimal premium,
            PolicyCalculationContext context,
            IEnumerable<RiskFactorConfiguration> riskFactors);
    }

}
