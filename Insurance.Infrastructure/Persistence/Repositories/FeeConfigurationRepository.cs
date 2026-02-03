using Insurance.Domain.Metadata;
using Insurance.Infrastructure.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Repositories
{
    public class FeeConfigurationRepository : IFeeConfigurationRepository
    {
        private readonly InsuranceDbContext _dbContext;

        public FeeConfigurationRepository(InsuranceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(FeeConfiguration feeConfiguration, CancellationToken cancellationToken)
        {
            var entity = FeeConfigurationMapper.ToEntity(feeConfiguration);
            await _dbContext.FeeConfigurations.AddAsync(entity, cancellationToken);
        }

        public async Task<FeeConfiguration?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.FeeConfigurations.FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
            return entity != null ? FeeConfigurationMapper.ToDomain(entity) : null;
        }

        public async Task UpdateAsync(FeeConfiguration feeConfiguration, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.FeeConfigurations.FirstAsync(f => f.Id == feeConfiguration.Id, cancellationToken);
            
            entity.Name = feeConfiguration.Name;
            entity.Percentage = feeConfiguration.Percentage;
            entity.EffectiveFrom = feeConfiguration.EffectiveFrom;
            entity.EffectiveTo = feeConfiguration.EffectiveTo;
            entity.IsActive = feeConfiguration.IsActive;

        }
    }
}
