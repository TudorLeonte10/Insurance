using AutoMapper;
using AutoMapper.QueryableExtensions;
using Insurance.Application.Clients.DTOs;
using Insurance.Application.Common.Paging;
using Insurance.Infrastructure.Persistence.Mappers;
using Insurance.Domain.Clients;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Repositories
{
    [ExcludeFromCodeCoverage]
    public class ClientRepository : IClientRepository
    {
        private readonly InsuranceDbContext _dbContext;

        public ClientRepository(InsuranceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Client?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var entity = await _dbContext.Clients
                .Include(c => c.Buildings)
                .FirstOrDefaultAsync(c => c.Id == id, ct);

            return entity is null ? null : ClientMapper.ToDomain(entity);
        }

        public async Task AddAsync(Client client, CancellationToken ct)
        {
            var entity = ClientMapper.ToEntity(client);
            await _dbContext.Clients.AddAsync(entity, ct);
        }

        public Task UpdateAsync(Client client, CancellationToken ct)
        {
            var entity = ClientMapper.ToEntity(client);
            _dbContext.Clients.Update(entity);
            return Task.CompletedTask;
        }

    }
}

