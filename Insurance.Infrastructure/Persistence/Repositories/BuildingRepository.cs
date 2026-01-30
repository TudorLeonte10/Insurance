using AutoMapper;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Buildings.DTOs;
using Insurance.Domain.Buildings;
using Insurance.Domain.RiskIndicators;
using Insurance.Infrastructure.Persistence.Entities;
using Insurance.Infrastructure.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Repositories
{
    [ExcludeFromCodeCoverage]
    public class BuildingRepository : IBuildingRepository
    {
        private readonly InsuranceDbContext _dbContext;

        public BuildingRepository(InsuranceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Building?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var entity = await _dbContext.Buildings
                .FirstOrDefaultAsync(b => b.Id == id, ct);

            return entity is null ? null : BuildingMapper.ToDomain(entity);
        }

        public async Task AddAsync(
            Building building,
            IReadOnlyCollection<RiskIndicatorType> riskIndicators,
            CancellationToken ct)
        {
            var entity = BuildingMapper.ToEntity(building);

            await _dbContext.Buildings.AddAsync(entity, ct);

            if (riskIndicators.Any())
            {
                var indicators = riskIndicators
                    .Select(r => new BuildingRiskIndicatorEntity
                    {
                        Id = Guid.NewGuid(),
                        BuildingId = entity.Id,
                        RiskIndicator = r.ToString()
                    });

                await _dbContext.BuildingRiskIndicators
                    .AddRangeAsync(indicators, ct);
            }
        }

        public async Task UpdateAsync(Building building, CancellationToken ct)
{
            var entity = await _dbContext.Buildings
                .FirstOrDefaultAsync(b => b.Id == building.Id, ct);

            if (entity is null)
                throw new InvalidOperationException("Building not found");

            entity.Type = building.Type.ToString();
            entity.Street = building.Street;
            entity.Number = building.Number;
            entity.ConstructionYear = building.ConstructionYear;
            entity.NumberOfFloors = building.NumberOfFloors;
            entity.SurfaceArea = building.SurfaceArea;
            entity.InsuredValue = building.InsuredValue;
     }
    }
}


