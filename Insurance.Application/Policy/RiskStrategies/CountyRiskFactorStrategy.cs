using Insurance.Application.Policy.Services;
using Insurance.Domain.Buildings;
using Insurance.Domain.Metadata;
using Insurance.Domain.Metadata.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.RiskStrategies
{
    public class CountyRiskFactorStrategy : IRiskFactorStrategy
    {

        public decimal Apply(decimal premium,
            PolicyCalculationContext context,
            RiskFactorConfiguration risk)
        {
            if (context.CountyId.ToString() != risk.ReferenceId)
                return premium;

            return premium * (1 + risk.AdjustmentPercentage);
        }

        public bool CanHandle(RiskFactorLevel level)
             => level == RiskFactorLevel.County;
    }

}
