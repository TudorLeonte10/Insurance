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
            var sortedFees = fees.OrderBy(f => f.Type).ThenBy(f => f.Id);
            foreach (var fee in sortedFees)
            {
                var strategy = _strategies.FirstOrDefault(s => s.CanHandle(fee));

                if (strategy != null)
                {
                    premium = strategy.Apply(premium, context, fee);
                }
            }

            return premium;
        }
    }

}

