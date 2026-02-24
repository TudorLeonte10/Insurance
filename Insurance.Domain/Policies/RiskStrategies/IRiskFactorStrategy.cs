using Insurance.Application.Policy.Services;
using Insurance.Domain.Buildings;
using Insurance.Domain.Metadata;
using Insurance.Domain.Metadata.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.RiskStrategies
{
    public interface IRiskFactorStrategy
    {
        bool CanHandle(RiskFactorLevel level);

        decimal Apply(
            decimal premium,
            PolicyCalculationContext context,
            RiskFactorConfiguration risk);
    }
}
