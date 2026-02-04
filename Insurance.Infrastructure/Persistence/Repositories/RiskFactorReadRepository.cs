using AutoMapper;
using AutoMapper.QueryableExtensions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Common.Paging;
using Insurance.Application.Metadata.RiskFactors.DTOs;
using Insurance.Domain.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Insurance.Infrastructure.Persistence.Repositories
{
    public class RiskFactorReadRepository : IRiskFactorReadRepository
    {
        private readonly InsuranceDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly TimeProvider _timeProvider;
        public RiskFactorReadRepository(InsuranceDbContext dbContext, IMapper mapper, TimeProvider timeProvider)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _timeProvider = timeProvider;
        }
        public async Task<PagedResult<RiskFactorConfigurationDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var totalItems = await _dbContext.RiskFactorConfigurations.CountAsync(cancellationToken);
            var items = await _dbContext.RiskFactorConfigurations
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<RiskFactorConfigurationDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new PagedResult<RiskFactorConfigurationDto>(items, pageNumber, pageSize, totalItems);

        }

        public async Task<IReadOnlyList<RiskFactorConfiguration>> GetActiveAsync(
       CancellationToken cancellationToken)
        {
            var now = _timeProvider.GetUtcNow();

            return await _dbContext.RiskFactorConfigurations
                .AsNoTracking()
                .Where(r => r.IsActive)
                .Select(r => new RiskFactorConfiguration
                {
                    Id = r.Id,
                    Level = r.Level,
                    ReferenceId = r.ReferenceId,
                    AdjustmentPercentage = r.AdjustmentPercentage,
                    IsActive = r.IsActive
                })
                .ToListAsync(cancellationToken);
        }
    }
}
