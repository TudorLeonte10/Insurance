using AutoMapper;
using AutoMapper.QueryableExtensions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Buildings.DTOs;
using Insurance.Domain.Buildings;
using Insurance.Domain.RiskIndicators;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Repositories
{
    [ExcludeFromCodeCoverage]
    public class BuildingReadRepository : IBuildingReadRepository
    {
        private readonly InsuranceDbContext _dbContext;
        private readonly IMapper _mapper;

        public BuildingReadRepository(
            InsuranceDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<BuildingDetailsDto?> GetByIdAsync(
            Guid buildingId,
            CancellationToken ct)
        {
            return await _dbContext.Buildings
                .AsNoTracking()
                .Where(b => b.Id == buildingId)
                .ProjectTo<BuildingDetailsDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);
        }

        public async Task<IReadOnlyList<BuildingDetailsDto>> GetByClientIdAsync(
            Guid clientId,
            CancellationToken ct)
        {
            return await _dbContext.Buildings
                .AsNoTracking()
                .Where(b => b.ClientId == clientId)
                .ProjectTo<BuildingDetailsDto>(_mapper.ConfigurationProvider)
                .ToListAsync(ct);
        }

        public async Task<BuildingGeoContextDto?> GetGeoContextAsync(Guid buildingId, CancellationToken cancellationToken)
        {
            return await _dbContext.Buildings
                .AsNoTracking()
                .Include(b => b.City)
                    .ThenInclude(c => c.County)
                        .ThenInclude(co => co.Country)
                .Where(b => b.Id == buildingId)
                .Select(b => new BuildingGeoContextDto
                {
                    BuildingId = b.Id,
                    CityId = b.CityId,
                    CountyId = b.City.CountyId,
                    CountryId = b.City.County.CountryId,
                    BuildingType = b.Type,
                    RiskIndicators = b.RiskIndicators!
                        .Select(ri => Enum.Parse<RiskIndicatorType>(ri.RiskIndicator))
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> IsOwnedByClientAsync(Guid buildingId, Guid clientId, CancellationToken cancellationToken)
        {
            return await _dbContext.Buildings
                .AsNoTracking()
                .AnyAsync(b => b.Id == buildingId && b.ClientId == clientId, cancellationToken);
        }
    }

}
