using AutoMapper;
using AutoMapper.QueryableExtensions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Common.Paging;
using Insurance.Application.FeeConfiguration.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Repositories
{
    public class FeeConfigurationReadRepository : IFeeConfigurationReadRepository
    {
        private readonly InsuranceDbContext _dbContext;
        private readonly IMapper _mapper;   
        public FeeConfigurationReadRepository(InsuranceDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
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
    }
}
