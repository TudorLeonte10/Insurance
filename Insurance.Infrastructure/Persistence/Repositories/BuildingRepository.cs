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
        private readonly IMapper _mapper;

        public BuildingRepository(InsuranceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<Building>> GetAllBuildingsByClientId(Guid clientId, CancellationToken cancellationToken)
        {
            return await _context.Buildings
                .Where(b => b.ClientId == clientId)
                .Include(b => b.City)
                    .ThenInclude(c => c.County)
                        .ThenInclude(co => co.Country)
                .Include(b => b.RiskIndicators)
                .ToListAsync(cancellationToken);
        }
    }
}
