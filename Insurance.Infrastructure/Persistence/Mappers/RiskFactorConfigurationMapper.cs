using Insurance.Domain.Metadata;
using Insurance.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Mappers
{
    public static class RiskFactorConfigurationMapper
    {
        public static RiskFactorConfiguration ToDomain(
            RiskFactorConfigurationEntity entity)
        {
            return new RiskFactorConfiguration
            {
                Id = entity.Id,
                Level = entity.Level,
                ReferenceId = entity.ReferenceId,
                AdjustmentPercentage = entity.AdjustmentPercentage,
                IsActive = entity.IsActive
            };
        }

        public static RiskFactorConfigurationEntity ToEntity(
            RiskFactorConfiguration domain)
        {
            return new RiskFactorConfigurationEntity
            {
                Id = domain.Id,
                Level = domain.Level,
                ReferenceId = domain.ReferenceId,
                AdjustmentPercentage = domain.AdjustmentPercentage,
                IsActive = domain.IsActive
            };
        }
    }

}
