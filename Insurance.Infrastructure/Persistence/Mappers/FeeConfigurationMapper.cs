using Insurance.Domain.Metadata;
using Insurance.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Mappers
{
    public static class FeeConfigurationMapper
    {
        public static FeeConfiguration ToDomain(FeeConfigurationEntity entity)
        {
            return new FeeConfiguration
            {
                Id = entity.Id,
                Name = entity.Name,
                Type = entity.Type,
                Percentage = entity.Percentage,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
                IsActive = entity.IsActive
            };
        }

        public static FeeConfigurationEntity ToEntity(FeeConfiguration domain)
        {
            return new FeeConfigurationEntity
            {
                Id = domain.Id,
                Name = domain.Name,
                Type = domain.Type,
                Percentage = domain.Percentage,
                EffectiveFrom = domain.EffectiveFrom,
                EffectiveTo = domain.EffectiveTo,
                IsActive = domain.IsActive
            };
        }
    }
}
