using Insurance.Application.Policy.Services;
using Insurance.Domain.Buildings;
using Insurance.Domain.Metadata;
using Insurance.Domain.Metadata.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.FeeStrategies
{
    public class FeeCalculator : IFeeCalculator
    {
        private readonly IEnumerable<IFeeStrategy> _strategies;

        public FeeCalculator(IEnumerable<IFeeStrategy> strategies)
        {
            _strategies = strategies;
        }

        public decimal ApplyFees(
            decimal premium,
            PolicyCalculationContext context,
            IEnumerable<FeeConfiguration> fees)
        {
            foreach (var fee in fees)
            {
                foreach (var strategy in _strategies.Where(s => s.CanHandle(fee)))
                {
                    premium = strategy.Apply(premium, context, fee);
                }
            }

            return premium;
        }
    }

}

