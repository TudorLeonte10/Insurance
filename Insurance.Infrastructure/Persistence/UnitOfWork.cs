using Insurance.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InsuranceDbContext _dbContext;

        public UnitOfWork(InsuranceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
