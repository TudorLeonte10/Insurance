using Insurance.Domain.Metadata;
using Insurance.Infrastructure.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Repositories
{
    [ExcludeFromCodeCoverage]
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly InsuranceDbContext _dbContext;
        public CurrencyRepository(InsuranceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Currency currency, CancellationToken cancellationToken)
        {
            var entity = CurrencyMapper.ToEntity(currency);
            await _dbContext.Currencies.AddAsync(entity, cancellationToken);

        }

        public async Task<Currency?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Currencies.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
            return entity == null ? null : CurrencyMapper.ToDomain(entity);
        }

        public async Task UpdateAsync(Currency currency, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Currencies.FirstAsync(f => f.Id == currency.Id, cancellationToken);

            entity.Code = currency.Code;
            entity.Name = currency.Name;
            entity.ExchangeRateToBase = currency.ExchangeRateToBase;
            entity.IsActive = currency.IsActive;
            
        }
    }
}
