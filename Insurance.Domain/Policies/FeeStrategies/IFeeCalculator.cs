using Insurance.Application.Policy.Services;
using Insurance.Domain.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.FeeStrategies
{
    public interface IFeeCalculator
    {
        decimal ApplyFees(
            decimal premium,
            PolicyCalculationContext context,
            IEnumerable<FeeConfiguration> fees);
    }
}
