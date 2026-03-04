using Insurance.Application.Policy.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Services
{
    public interface IAnomalyFeatureService
    {
        Task<AnomalyFeatureDto> BuildAsync(
            Domain.Policies.Policy policy,
            PremiumCalculationResult premiumResult,
            Guid brokerId,
            Guid buildingId,
            Guid clientId,
            CancellationToken cancellationToken
            );
    }
}
