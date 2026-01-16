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
        public ClientRepository(InsuranceDbContext dbContext) : base(dbContext) { }

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
                .Select(c => new ClientDetailsDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    IdentificationNumber = c.IdentificationNumber,
                    Type = c.Type,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    Address = c.Address
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<ClientDetailsDto>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

    }
}
