using Insurance.Domain.Metadata;
using Insurance.Infrastructure.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Repositories
{
    public class RiskFactorConfigurationRepository : IRiskFactorConfigurationRepository
    {
        private readonly InsuranceDbContext _dbContext;
        public RiskFactorConfigurationRepository(InsuranceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(RiskFactorConfiguration riskFactorConfiguration, CancellationToken ct)
        {
            var entity = RiskFactorConfigurationMapper.ToEntity(riskFactorConfiguration);
            await _dbContext.RiskFactorConfigurations.AddAsync(entity, ct);
        }

        public async Task UpdateAsync(RiskFactorConfiguration riskFactorConfiguration, CancellationToken ct)
        {
            var entity = await _dbContext.RiskFactorConfigurations
                .FirstAsync(r => r.Id == riskFactorConfiguration.Id, ct);

            entity.Level = riskFactorConfiguration.Level;
            entity.AdjustmentPercentage = riskFactorConfiguration.AdjustmentPercentage;
            entity.ReferenceId = riskFactorConfiguration.ReferenceId;
            

        }

        public async Task<RiskFactorConfiguration?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var entity = await _dbContext.RiskFactorConfigurations
                .FirstOrDefaultAsync(r => r.Id == id, ct);

            if(entity == null)
            {
                return null;
            }

            var domainModel = RiskFactorConfigurationMapper.ToDomain(entity);
            return domainModel;
        }
    }
}
