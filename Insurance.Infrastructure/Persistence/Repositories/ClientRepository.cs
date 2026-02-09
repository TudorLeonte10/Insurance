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
using Insurance.Domain.Exceptions;
using Insurance.Application.Exceptions;

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

        public async Task UpdateAsync(Client client, CancellationToken ct)
        {
            var entity = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Id == client.Id);

            if (entity is null)
                throw new NotFoundException($"Entity with id {client.Id} not found");

            entity.Name = client.Name;
            entity.Email = client.Email;
            entity.PhoneNumber = client.PhoneNumber;
            entity.Address = client.Address;
            entity.IdentificationNumber = client.IdentificationNumber;
        }

    }
}

