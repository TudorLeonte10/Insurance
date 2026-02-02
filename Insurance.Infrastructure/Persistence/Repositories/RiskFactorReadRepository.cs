using AutoMapper;
using AutoMapper.QueryableExtensions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Common.Paging;
using Insurance.Application.Metadata.RiskFactors.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Repositories
{
    public class RiskFactorReadRepository : IRiskFactorReadRepository
    {
        private readonly InsuranceDbContext _dbContext;
        private readonly IMapper _mapper;
        public RiskFactorReadRepository(InsuranceDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;   
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
    }
}
