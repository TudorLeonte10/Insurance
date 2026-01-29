using AutoMapper;
using AutoMapper.QueryableExtensions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Buildings.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Repositories
{
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
    }

}
