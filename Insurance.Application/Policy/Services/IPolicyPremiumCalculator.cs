using Insurance.Domain.Buildings;
using Insurance.Domain.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Services
{
    public interface IPolicyPremiumCalculator
    {
        decimal Calculate(
            decimal basePremium,
            PolicyCalculationContext context,
            IEnumerable<FeeConfiguration> fees,
            IEnumerable<RiskFactorConfiguration> riskFactors);
    }
}
