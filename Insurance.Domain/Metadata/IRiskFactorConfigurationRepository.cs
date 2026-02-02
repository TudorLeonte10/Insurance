using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Domain.Metadata
{
    public interface IRiskFactorConfigurationRepository
    {
        Task<RiskFactorConfiguration?> GetByIdAsync(Guid id, CancellationToken ct);
        Task AddAsync(RiskFactorConfiguration riskFactorConfiguration, CancellationToken ct);
        Task UpdateAsync(RiskFactorConfiguration riskFactorConfiguration, CancellationToken ct);
    }
}
