#nullable enable
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Domain.Abstractions.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken);

        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);

        Task AddAsync(T entity, CancellationToken cancellationToken);

        Task UpdateAsync(T entity, CancellationToken cancellationToken);

        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}

