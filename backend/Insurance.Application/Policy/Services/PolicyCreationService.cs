using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Exceptions;
using Insurance.Application.Policy.DTOs;
using Insurance.Domain.Clients;
using Insurance.Domain.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Services
{
    public class PolicyCreationService : IPolicyCreationService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IBuildingReadRepository _buildingReadRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IFeeConfigurationReadRepository _feeReadRepository;
        private readonly IRiskFactorReadRepository _riskReadRepository;
        private readonly IPolicyPremiumCalculator _premiumCalculator;
        private readonly TimeProvider _timeProvider;

        public PolicyCreationService(
            IClientRepository clientRepository,
            IBuildingReadRepository buildingReadRepository,
            ICurrencyRepository currencyRepository,
            IFeeConfigurationReadRepository feeReadRepository,
            IRiskFactorReadRepository riskReadRepository,
            IPolicyPremiumCalculator premiumCalculator,
            TimeProvider timeProvider)
        {
            _clientRepository = clientRepository;
            _buildingReadRepository = buildingReadRepository;
            _currencyRepository = currencyRepository;
            _feeReadRepository = feeReadRepository;
            _riskReadRepository = riskReadRepository;
            _premiumCalculator = premiumCalculator;
            _timeProvider = timeProvider;
        }
        public async Task<PolicyCreationResult> CreatePolicyAsync(
    CreatePolicyDto dto,
    Guid brokerId,
    CancellationToken cancellationToken)
        {
            await ValidateClientOwnershipAsync(dto.ClientId, brokerId, cancellationToken);
            await ValidateBuildingOwnershipAsync(dto.BuildingId, dto.ClientId, cancellationToken);

            var currency = await GetAndValidateCurrencyAsync(dto.CurrencyId, cancellationToken);

            var geoContext = await _buildingReadRepository
                .GetGeoContextAsync(dto.BuildingId, cancellationToken);

            var fees = await _feeReadRepository.GetActiveAsync(cancellationToken);
            var riskFactors = await _riskReadRepository.GetActiveAsync(cancellationToken);

            var calculationContext = new PolicyCalculationContext
            {
                CityId = geoContext!.CityId,
                CountyId = geoContext.CountyId,
                CountryId = geoContext.CountryId,
                BuildingType = geoContext.BuildingType,
                RiskIndicators = geoContext.RiskIndicators
            };

            var premiumResult = await CalculateFinalPremium(currency, dto, cancellationToken);

            var now = _timeProvider.GetUtcNow().UtcDateTime;

            var policy = Domain.Policies.Policy.CreateDraft(
                clientId: dto.ClientId,
                buildingId: dto.BuildingId,
                brokerId: brokerId,
                currencyId: dto.CurrencyId,
                basePremium: dto.BasePremium,
                finalPremium: premiumResult.PremiumInCurrency,
                startDate: dto.StartDate,
                endDate: dto.EndDate,
                policyNumber: GeneratePolicyNumber(),
                now: now);

            return new PolicyCreationResult
            {
                Policy = policy,
                Country = geoContext.CountryName,
                County = geoContext.CountyName,
                City = geoContext.CityName,
                BrokerCode = geoContext.BrokerCode,
                Currency = currency.Name,
                Status = policy.Status.ToString(),
                BuildingType = geoContext.BuildingType.ToString(),
                FinalPremium = premiumResult.PremiumInCurrency,
                FinalPremiumInBase = premiumResult.PremiumInBase,
                CreatedAt = now
            };
        }


        private async Task ValidateClientOwnershipAsync(Guid clientId, Guid brokerId, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetByIdAsync(clientId, cancellationToken);
            if (client == null)
            {
                throw new NotFoundException("Client does not exist.");
            }

            if (client.BrokerId != brokerId)
            {
                throw new ForbiddenException("Client does not belong to the broker.");
            }
        }

        private async Task ValidateBuildingOwnershipAsync(Guid buildingId, Guid clientId, CancellationToken cancellationToken)
        {
            var building = await _buildingReadRepository.GetByIdAsync(buildingId, cancellationToken);
            if (building == null)
            {
                throw new NotFoundException("Building does not exist.");
            }
            if (building.ClientId != clientId)
            {
                throw new ForbiddenException("Building does not belong to the client.");
            }
        }

        private async Task<Currency> GetAndValidateCurrencyAsync(Guid currencyId, CancellationToken cancellationToken)
        {
            var currency = await _currencyRepository.GetByIdAsync(currencyId, cancellationToken);
            if (currency == null)
            {
                throw new NotFoundException("Currency does not exist.");
            }
            return currency;
        }

        private async Task<PremiumCalculationResult> CalculateFinalPremium(
    Currency currency,
    CreatePolicyDto dto,
    CancellationToken cancellationToken)
        {
            var fees = await _feeReadRepository.GetActiveAsync(cancellationToken);
            var riskFactors = await _riskReadRepository.GetActiveAsync(cancellationToken);

            var geoContext = await _buildingReadRepository
                .GetGeoContextAsync(dto.BuildingId, cancellationToken);

            var calculationContext = new PolicyCalculationContext
            {
                CityId = geoContext!.CityId,
                CountyId = geoContext.CountyId,
                CountryId = geoContext.CountryId,
                BuildingType = geoContext.BuildingType,
                RiskIndicators = geoContext.RiskIndicators
            };

            var premiumBase = _premiumCalculator
                .Calculate(dto.BasePremium, calculationContext, fees, riskFactors);

            var finalPremium = CurrencyConverter
                .ConvertFromBase(premiumBase, currency);

            return new PremiumCalculationResult
            {
                PremiumInBase = premiumBase,
                PremiumInCurrency = finalPremium
            };
        }


        private static string GeneratePolicyNumber()
            => $"POL-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid():N}";
    }

    public class PremiumCalculationResult
    {
        public decimal PremiumInBase { get; init; }
        public decimal PremiumInCurrency { get; init; }
    }
}
