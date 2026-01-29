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
    public class ClientReadRepository : IClientReadRepository
    {
        private readonly InsuranceDbContext _dbContext;
        private readonly IMapper _mapper;

        public ClientReadRepository(
            InsuranceDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ClientDetailsDto?> GetByIdAsync(
            Guid clientId,
            CancellationToken ct)
        {
            return await _dbContext.Clients
                .AsNoTracking()
                .Where(c => c.Id == clientId)
                .ProjectTo<ClientDetailsDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);
        }
    }
}
