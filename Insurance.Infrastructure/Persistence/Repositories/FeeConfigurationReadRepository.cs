using AutoMapper;
using AutoMapper.QueryableExtensions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Common.Paging;
using Insurance.Application.Metadata.FeeConfiguration.DTOs;
using Insurance.Domain.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Insurance.Infrastructure.Persistence.Repositories
{
    public class FeeConfigurationReadRepository : IFeeConfigurationReadRepository
    {
        private readonly InsuranceDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly TimeProvider _timeProvider;
        public FeeConfigurationReadRepository(InsuranceDbContext dbContext, IMapper mapper, TimeProvider timeProvider)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _timeProvider = timeProvider;
        }
        public async Task<PagedResult<FeeConfigurationDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var totalItems = await _dbContext.FeeConfigurations.CountAsync(cancellationToken);
            var items = await _dbContext.FeeConfigurations
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<FeeConfigurationDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new PagedResult<FeeConfigurationDto>(items, pageNumber, pageSize, totalItems);
        }

        public async Task<IReadOnlyList<FeeConfiguration>> GetActiveAsync(CancellationToken cancellationToken)
        {
            var now = _timeProvider.GetUtcNow();

            return await _dbContext.FeeConfigurations
                .AsNoTracking()
                .Where(f =>
                    f.IsActive &&
                    f.EffectiveFrom <= now &&
                    (f.EffectiveTo == null || f.EffectiveTo >= now))
                .Select(f => new FeeConfiguration
                {
                    Id = f.Id,
                    Name = f.Name,
                    Type = f.Type,
                    RiskIndicatorType = f.RiskIndicatorType,
                    Percentage = f.Percentage,
                    EffectiveFrom = f.EffectiveFrom,
                    EffectiveTo = f.EffectiveTo,
                    IsActive = f.IsActive
                })
                .ToListAsync(cancellationToken);
        }
    }
}
