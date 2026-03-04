using Insurance.Application.Exceptions;
using Insurance.Domain.Brokers;
using Insurance.Infrastructure.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Repositories
{
    [ExcludeFromCodeCoverage]
    public class BrokerRepository : IBrokerRepository
    {
        private readonly InsuranceDbContext _dbContext;
        public BrokerRepository(InsuranceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Broker broker, CancellationToken cancellationToken)
        {
            var brokerEntity = BrokerMapper.ToEntity(broker);
            await _dbContext.Brokers.AddAsync(brokerEntity, cancellationToken);
        }

        public async Task<Broker?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var brokerEntity = await _dbContext.Brokers.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
            return brokerEntity == null ? null : BrokerMapper.ToDomain(brokerEntity);
        }

        public async Task UpdateAsync(Broker broker, CancellationToken cancellationToken)
        {
            var brokerEntity = await _dbContext.Brokers
        .FirstAsync(b => b.Id == broker.Id, cancellationToken);

            brokerEntity.Name = broker.Name;
            brokerEntity.Email = broker.Email;
            brokerEntity.Phone = broker.Phone;
            brokerEntity.IsActive = broker.IsActive;

        }
    }
}
