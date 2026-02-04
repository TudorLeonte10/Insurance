using Insurance.Domain.Policies;
using Insurance.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Mappers
{
    public static class PolicyMapper
    {
        public static PolicyEntity ToEntity(Policy policy)
        {
            return new PolicyEntity
            {
                Id = policy.Id,
                PolicyNumber = policy.PolicyNumber,
                Status = policy.Status,

                ClientId = policy.ClientId,
                BuildingId = policy.BuildingId,
                BrokerId = policy.BrokerId,
                CurrencyId = policy.CurrencyId,

                BasePremium = policy.BasePremium,
                FinalPremium = policy.FinalPremium,

                StartDate = policy.StartDate,
                EndDate = policy.EndDate,

                CreatedAt = policy.CreatedAt,
                ActivatedAt = policy.ActivatedAt,
                CancelledAt = policy.CancelledAt,
                CancellationReason = policy.CancellationReason
            };
        }

        public static Policy ToDomain(PolicyEntity entity)
        {
            return Policy.Rehydrate(
                entity.Id,
                entity.PolicyNumber,
                entity.Status,
                entity.ClientId,
                entity.BuildingId,
                entity.BrokerId,
                entity.CurrencyId,
                entity.BasePremium,
                entity.FinalPremium,
                entity.StartDate,
                entity.EndDate,
                entity.CreatedAt,
                entity.ActivatedAt,
                entity.CancelledAt,
                entity.CancellationReason
            );
        }
    }

}
