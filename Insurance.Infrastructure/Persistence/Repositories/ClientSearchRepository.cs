using AutoMapper;
using AutoMapper.QueryableExtensions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Clients.DTOs;
using Insurance.Application.Common.Paging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Repositories
{
    public class ClientSearchRepository : IClientSearchRepository
    {
        private readonly InsuranceDbContext _dbContext;
        private readonly IMapper _mapper;

        public ClientSearchRepository(
            InsuranceDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PagedResult<ClientDetailsDto>> SearchAsync(
            string? name,
            string? identifier,
            int pageNumber,
            int pageSize,
            CancellationToken ct)
        {
            var query = _dbContext.Clients.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(c => c.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(identifier))
                query = query.Where(c => c.IdentificationNumber == identifier);

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderBy(c => c.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ClientDetailsDto>(_mapper.ConfigurationProvider)
                .ToListAsync(ct);

            return new PagedResult<ClientDetailsDto>(
                items, pageNumber, pageSize, totalCount);
        }
    }

}
