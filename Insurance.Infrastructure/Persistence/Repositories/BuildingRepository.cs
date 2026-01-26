using AutoMapper;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Buildings.DTOs;
using Insurance.Domain.Buildings;
using Insurance.Domain.RiskIndicators;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Repositories
{
    public class BuildingRepository : IBuildingRepository
    {
        private readonly InsuranceDbContext _context;

        public BuildingRepository(InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Building>> GetAllBuildingsByClientIdAsync(Guid clientId, CancellationToken cancellationToken)
        {
            return await _context.Buildings
                .Where(b => b.ClientId == clientId)
                .Include(b => b.City)
                    .ThenInclude(c => c.County)
                        .ThenInclude(co => co.Country)
                .Include(b => b.RiskIndicators)
                .ToListAsync(cancellationToken);
        }

        public async Task<Building?> GetBuildingByIdAsync(Guid buildingId, CancellationToken cancellationToken)
        {
            return await _context.Buildings
                .AsNoTracking()
                .Include(b => b.City)
                    .ThenInclude(c => c.County)
                        .ThenInclude(co => co.Country)
                .Include(b => b.RiskIndicators)
                .FirstOrDefaultAsync(b => b.Id == buildingId, cancellationToken);
        }

        public async Task AddBuildingAsync(Building building, CancellationToken cancellationToken)
        {
            await _context.Buildings.AddAsync(building, cancellationToken);
        }

        public async Task UpdateAsync(Building building,IEnumerable<RiskIndicatorType> riskIndicators, CancellationToken cancellationToken)
        {
            _context.Attach(building);

            _context.Entry(building).State = EntityState.Modified;

            var existingRisks = _context.Set<BuildingRiskIndicator>()
                .Where(r => r.BuildingId == building.Id);

            _context.RemoveRange(existingRisks);

            foreach (var risk in riskIndicators.Distinct())
            {
                _context.Add(new BuildingRiskIndicator
                {
                    Id = Guid.NewGuid(),
                    BuildingId = building.Id,
                    RiskIndicator = risk
                });
            }
        }

    }
}
