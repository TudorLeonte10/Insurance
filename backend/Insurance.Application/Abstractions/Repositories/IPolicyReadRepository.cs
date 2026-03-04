using Insurance.Application.Policy.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Abstractions.Repositories
{
    public interface IPolicyReadRepository
    {
        Task<PolicyDetailsDto?> GetByIdAsync(Guid policyId, CancellationToken ct);
        Task<decimal> GetBrokerAveragePremiumAsync(Guid brokerId, CancellationToken cancellationToken);
        Task<decimal> GetBrokerGlobalAveragePremiumAsync(CancellationToken cancellationToken);
        Task<int> GetPoliciesOfClientFromLastYearAsync(Guid clientId, CancellationToken cancellationToken);
        Task<decimal> GetClientAverageInsuredValue(Guid clientId, CancellationToken cancellationToken);
        Task<decimal> GetClientAveragePremiumRatioAsync(Guid clientId, CancellationToken cancellationToken);
        Task<decimal> GetClientGlobalAverageInsuredValue(CancellationToken cancellationToken);
    }

}
