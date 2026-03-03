using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Domain.Policies
{
    public interface IPolicyRepository
    {
        Task<Policy?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(Policy policy, CancellationToken cancellationToken);
        Task UpdateAsync(Policy policy, CancellationToken cancellationToken);
      
    }
}
