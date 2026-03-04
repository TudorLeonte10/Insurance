using Insurance.Application.Policy.Services;
using Insurance.Domain.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.FeeStrategies
{
    public interface IFeeStrategy
    {
        bool CanHandle(FeeConfiguration fee);
        decimal Apply(
            decimal premium,
            PolicyCalculationContext context,
            FeeConfiguration fee);
    }

}
