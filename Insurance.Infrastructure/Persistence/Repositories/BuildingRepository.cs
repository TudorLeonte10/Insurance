using AutoMapper;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Buildings.DTOs;
using Insurance.Domain.Buildings;
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
        
        public void UpdateBuilding(Building building, CancellationToken cancellationToken)
        {
            _context.Buildings.Update(building);
        }
    }
}
