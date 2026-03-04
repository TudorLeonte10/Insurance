using Insurance.Domain.Buildings;
using Insurance.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Mappers
{
    [ExcludeFromCodeCoverage]
    public static class BuildingMapper
    {
        public static Building ToDomain(BuildingEntity entity)
        {
            return Building.Rehydrate(
                entity.Id,
                entity.ClientId,
                entity.CityId,
                entity.Type,
                entity.Street,
                entity.Number,
                entity.ConstructionYear,
                entity.NumberOfFloors,
                entity.SurfaceArea,
                entity.InsuredValue
            );
        }

        public static BuildingEntity ToEntity(Building domain)
            => new()
            {
                Id = domain.Id,
                ClientId = domain.ClientId,
                CityId = domain.CityId,
                Type = domain.Type,
                Street = domain.Street,
                Number = domain.Number,
                ConstructionYear = domain.ConstructionYear,
                NumberOfFloors = domain.NumberOfFloors,
                SurfaceArea = domain.SurfaceArea,
                InsuredValue = domain.InsuredValue
            };
    }
}
