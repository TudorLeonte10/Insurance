using Insurance.Application.Policy.Services;
using Insurance.Domain.Metadata;
using Insurance.Domain.Metadata.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.FeeStrategies
{
    public class PercentageFeeStrategy : IFeeStrategy
    {
        public bool CanHandle(FeeConfiguration fee)
            => fee.Type == FeeType.AdminFee
            || fee.Type == FeeType.BrokerCommission;

        public decimal Apply(
            decimal premium,
            PolicyCalculationContext context,
            FeeConfiguration fee)
        {
            return premium * (1 + fee.Percentage);
        }
    }

}
