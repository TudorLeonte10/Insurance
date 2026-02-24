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
            var sortedRiskFactors = riskFactors.OrderBy(r => r.Level).ThenBy(r => r.Id);

            foreach (var risk in sortedRiskFactors)
            {
                var strategy = _strategies.FirstOrDefault(s => s.CanHandle(risk.Level));

                if (strategy != null)
                {
                    premium = strategy.Apply(premium, context, risk);
                }
            }

            return premium;
        }
    }

}
