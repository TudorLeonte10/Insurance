using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Domain.Metadata
{
    public interface IRiskFactorConfigurationRepository
    {
        Task<RiskFactorConfiguration?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(RiskFactorConfiguration riskFactorConfiguration, CancellationToken cancellationToken);
        Task UpdateAsync(RiskFactorConfiguration riskFactorConfiguration, CancellationToken cancellationToken);
    }
}
