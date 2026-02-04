using Insurance.Application.Policy.Services;
using Insurance.Domain.Metadata;
using Insurance.Domain.Metadata.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.FeeStrategies
{
    public class RiskIndicatorFeeStrategy : IFeeStrategy
    {
        public bool CanHandle(FeeConfiguration fee)
            => fee.Type == FeeType.RiskAdjustment
               && fee.RiskIndicatorType.HasValue;

        public decimal Apply(
            decimal premium,
            PolicyCalculationContext context,
            FeeConfiguration fee)
        {
            if (!context.RiskIndicators!.Contains(fee.RiskIndicatorType!.Value))
                return premium;

            return premium * (1 + fee.Percentage);
        }
    }

}
