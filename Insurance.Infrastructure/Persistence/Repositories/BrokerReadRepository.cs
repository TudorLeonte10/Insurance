using AutoMapper;
using AutoMapper.QueryableExtensions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Brokers.DTOs;
using Insurance.Application.Clients.DTOs;
using Insurance.Application.Common.Paging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Repositories
{
    [ExcludeFromCodeCoverage]
    public class BrokerReadRepository : IBrokerReadRepository
    {
        private readonly InsuranceDbContext _dbContext;
        private readonly IMapper _mapper;
        public BrokerReadRepository(InsuranceDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PagedResult<BrokerDetailsDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var totalRecords = await _dbContext.Brokers.CountAsync(cancellationToken);
            var brokers = await _dbContext.Brokers
                .AsNoTracking()
                .ProjectTo<BrokerDetailsDto>(_mapper.ConfigurationProvider)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<BrokerDetailsDto>(brokers, pageNumber, pageSize, totalRecords);
            
        }

        public async Task<BrokerDetailsDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Brokers
                .AsNoTracking()
                .Where(b => b.Id == id)
                .ProjectTo<BrokerDetailsDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }

    }
}
