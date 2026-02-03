using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Domain.Metadata
{
    public interface IFeeConfigurationRepository
    {
        Task<FeeConfiguration?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(FeeConfiguration feeConfiguration, CancellationToken cancellationToken);
        Task UpdateAsync(FeeConfiguration feeConfiguration, CancellationToken cancellationToken);
    }
}
