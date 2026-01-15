using Insurance.Domain.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly InsuranceDbContext DbContext;

        public Repository(InsuranceDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await DbContext.Set<T>().FindAsync(id, cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await DbContext.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await DbContext.Set<T>().FindAsync(id , cancellationToken) is not null;
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            await DbContext.Set<T>().AddAsync(entity, cancellationToken);
        }

        public Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            DbContext.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await DbContext.Set<T>().FindAsync(id, cancellationToken);

            if (entity is null)
                return;

            DbContext.Set<T>().Remove(entity);
        }
    }
}
