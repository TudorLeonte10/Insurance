using AutoMapper;
using AutoMapper.QueryableExtensions;
using Insurance.Application.Clients.DTOs;
using Insurance.Application.Common.Paging;
using Insurance.Domain.Abstractions.Repositories;
using Insurance.Domain.Clients;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Repositories
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        private readonly IMapper _mapper;
        public ClientRepository(InsuranceDbContext dbContext, IMapper mapper) : base(dbContext) 
        {
            _mapper = mapper;
        }

        public async Task<bool> ExistsByIdentifierAsync(string identifier, CancellationToken cancellationToken)
        {
            return await DbContext.Set<Client>().AnyAsync(
                c => c.IdentificationNumber == identifier, cancellationToken);
        }

        public async Task<Client?> GetByIdentificationNumberAsync(string identifier, CancellationToken cancellationToken)
        {
            return await DbContext.Set<Client>().FirstOrDefaultAsync(
                    c => c.IdentificationNumber == identifier, cancellationToken);
        }

        public async Task<PagedResult<ClientDetailsDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var query = DbContext.Set<Client>()
                .AsNoTracking();

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(c => c.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ClientDetailsDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new PagedResult<ClientDetailsDto>(items, pageNumber, pageSize, totalCount);

        }

        public async Task<PagedResult<ClientDetailsDto>> SearchAsync(string? name, string? identifier, int pageNumber, int pageSize,CancellationToken cancellationToken)
        {
            var query = DbContext.Set<Client>()
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(c => c.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(identifier))
                query = query.Where(c => c.IdentificationNumber == identifier);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(c => c.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ClientDetailsDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new PagedResult<ClientDetailsDto>(items, pageNumber, pageSize, totalCount);
        }

    }
}
