using Insurance.Application.Policy.FeeStrategies;
using Insurance.Application.Policy.RiskStrategies;
using Insurance.Domain.Buildings;
using Insurance.Domain.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Services
{
    public class PolicyPremiumCalculator : IPolicyPremiumCalculator
    {
        private readonly IFeeCalculator _feeCalculator;
        private readonly IRiskFactorCalculator _riskFactorCalculator;

        public PolicyPremiumCalculator(
            IFeeCalculator feeCalculator,
            IRiskFactorCalculator riskFactorCalculator)
        {
            _feeCalculator = feeCalculator;
            _riskFactorCalculator = riskFactorCalculator;
        }

        public decimal Calculate(
            decimal basePremium,
            PolicyCalculationContext context,
            IEnumerable<FeeConfiguration> fees,
            IEnumerable<RiskFactorConfiguration> riskFactors)
        {
            var premium = basePremium;

            premium = _feeCalculator.ApplyFees(premium, context, fees);

            premium = _riskFactorCalculator.ApplyRiskFactors(premium, context, riskFactors);

            return premium;
        }
    }

}
