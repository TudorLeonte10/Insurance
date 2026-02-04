using Insurance.Application.Policy.Services;
using Insurance.Domain.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.RiskStrategies
{
    public class RiskFactorCalculator : IRiskFactorCalculator
    {
        private readonly IEnumerable<IRiskFactorStrategy> _strategies;

        public RiskFactorCalculator(IEnumerable<IRiskFactorStrategy> strategies)
        {
            _strategies = strategies;
        }

        public decimal ApplyRiskFactors(
            decimal premium,
            PolicyCalculationContext context,
            IEnumerable<RiskFactorConfiguration> riskFactors)
        {
            foreach (var risk in riskFactors)
            {
                foreach (var strategy in _strategies.Where(s => s.CanHandle(risk.Level)))
                {
                    premium = strategy.Apply(premium, context, risk);
                }
            }

            return premium;
        }
    }

}
