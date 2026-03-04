using AutoMapper;
using AutoMapper.QueryableExtensions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Clients.DTOs;
using Insurance.Application.Common.Paging;
using Insurance.Application.Metadata.Currency.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Repositories
{
    [ExcludeFromCodeCoverage]
    public class CurrencyReadRepository : ICurrencyReadRepository
    {
        private readonly InsuranceDbContext _dbContext;
        private readonly IMapper _mapper;
        public CurrencyReadRepository(InsuranceDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<PagedResult<CurrencyDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var totalItems = await _dbContext.Currencies.CountAsync(cancellationToken);
            var currencies = await _dbContext.Currencies
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<CurrencyDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new PagedResult<CurrencyDto>(currencies, pageNumber, pageSize, totalItems);
        }
    
    }
}
