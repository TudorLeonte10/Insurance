using Insurance.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Domain.Policies
{
    public class Policy
    {
        public Guid Id { get; private set; }

        public string PolicyNumber { get; private set; } = string.Empty;

        public PolicyStatus Status { get; private set; }

        public Guid ClientId { get; private set; }
        public Guid BuildingId { get; private set; }
        public Guid BrokerId { get; private set; }
        public Guid CurrencyId { get; private set; }

        public decimal BasePremium { get; private set; }
        public decimal FinalPremium { get; private set; }

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime? ActivatedAt { get; private set; }
        public DateTime? CancelledAt { get; private set; }

        public string? CancellationReason { get; private set; }

        private Policy() { }

        public static Policy CreateDraft(
            Guid clientId,
            Guid buildingId,
            Guid brokerId,
            Guid currencyId,
            decimal basePremium,
            decimal finalPremium,
            DateTime startDate,
            DateTime endDate,
            string policyNumber,
            DateTime now)
        {
            if (basePremium <= 0)
            {
                throw new InvalidBasePremiumException("Base premium must be greater than zero.");
            }

            if (finalPremium < basePremium)
            {
                throw new InvalidFinalPremiumException("Final premium cannot be less than base premium.");
            }

            if (startDate >= endDate)
            {
                throw new InvalidPolicyTermException("Start date must be earlier than end date.");
            }

            return new Policy
            {
                Id = Guid.NewGuid(),
                PolicyNumber = policyNumber,
                Status = PolicyStatus.Draft,
                ClientId = clientId,
                BuildingId = buildingId,
                BrokerId = brokerId,
                CurrencyId = currencyId,
                BasePremium = basePremium,
                FinalPremium = finalPremium,
                StartDate = startDate,
                EndDate = endDate,
                CreatedAt = now
            };
        }

        public void Activate(DateTime now)
        {
            if(Status != PolicyStatus.Draft)
            {
                throw new InvalidPolicyTransitionException("Only draft policies can be activated.");
            }

            if (StartDate < now)
            {
                throw new InvalidPolicyTransitionException("Cannot activate policy with start date in the past.");
            }

            Status = PolicyStatus.Active;
            ActivatedAt = now;
        }

        public void Cancel(string? reason, DateTime now)
        {
            if (Status != PolicyStatus.Active)
            {
                throw new InvalidPolicyTransitionException("Only active policies can be cancelled.");
            }

            Status = PolicyStatus.Cancelled;
            CancellationReason = reason;
            CancelledAt = now;
        }

        public void Expire()
        {
            if (Status != PolicyStatus.Active)
            {
                throw new InvalidPolicyTransitionException("Only active policies can expire.");
            }
            Status = PolicyStatus.Expired;
        }

        public static Policy Rehydrate(
            Guid id,
            string policyNumber,
            PolicyStatus status,
            Guid clientId,
            Guid buildingId,
            Guid brokerId,
            Guid currencyId,
            decimal basePremium,
            decimal finalPremium,
            DateTime startDate,
            DateTime endDate,
            DateTime createdAt,
            DateTime? activatedAt,
            DateTime? cancelledAt,
            string? cancellationReason)
        {
            return new Policy
            {
                Id = id,
                PolicyNumber = policyNumber,
                Status = status,

                ClientId = clientId,
                BuildingId = buildingId,
                BrokerId = brokerId,
                CurrencyId = currencyId,

                BasePremium = basePremium,
                FinalPremium = finalPremium,

                StartDate = startDate,
                EndDate = endDate,

                CreatedAt = createdAt,
                ActivatedAt = activatedAt,
                CancelledAt = cancelledAt,
                CancellationReason = cancellationReason
            };
        }

    }

}
