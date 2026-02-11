using Insurance.Domain.Policies;
using Insurance.Infrastructure.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Repositories
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly InsuranceDbContext _dbContext;
        public PolicyRepository(InsuranceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Policy policy, CancellationToken cancellationToken)
        {
            var entity = PolicyMapper.ToEntity(policy);
            await _dbContext.Policies.AddAsync(entity, cancellationToken);
        }

        public async Task<Policy?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Policies
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            return entity == null ? null : PolicyMapper.ToDomain(entity);
        }

        public async Task UpdateAsync(Policy policy, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Policies
                .FirstAsync(p => p.Id == policy.Id, cancellationToken);

            PolicyMapper.UpdateActivation(entity, policy);
            
        }
    }
}
