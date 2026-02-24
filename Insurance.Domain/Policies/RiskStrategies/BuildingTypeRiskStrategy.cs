using Insurance.Application.Policy.Services;
using Insurance.Domain.Buildings;
using Insurance.Domain.Metadata;
using Insurance.Domain.Metadata.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.RiskStrategies
{
    public class BuildingTypeRiskStrategy : IRiskFactorStrategy
    {
        public bool CanHandle(RiskFactorLevel level)
            => level == RiskFactorLevel.BuildingType;

        public decimal Apply(decimal premium,
            PolicyCalculationContext context,
            RiskFactorConfiguration risk)
        {
            if (!Enum.TryParse<BuildingType>(
                 risk.ReferenceId,
                 out var buildingType))
                return premium;

            if (context.BuildingType != buildingType)
                return premium;

            return premium * (1 + risk.AdjustmentPercentage);
        }
    }
    }


