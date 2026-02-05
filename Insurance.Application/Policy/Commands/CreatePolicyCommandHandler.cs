using Insurance.Application.Abstractions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Buildings.DTOs;
using Insurance.Application.Exceptions;
using Insurance.Application.Policy.Services;
using Insurance.Domain.Brokers;
using Insurance.Domain.Clients;
using Insurance.Domain.Exceptions;
using Insurance.Domain.Metadata;
using Insurance.Domain.Policies;
using Insurance.Domain.RiskIndicators;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Insurance.Application.Policy.Commands
{
    public class CreatePolicyCommandHandler : IRequestHandler<CreatePolicyCommand, Guid>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IBuildingReadRepository _buildingReadRepository;
        private readonly IBrokerRepository _brokerRepository;
        private readonly ICurrencyRepository _currencyRepository;

        private readonly IFeeConfigurationReadRepository _feeReadRepository;
        private readonly IRiskFactorReadRepository _riskReadRepository;

        private readonly IPolicyPremiumCalculator _premiumCalculator;
        private readonly IPolicyRepository _policyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TimeProvider _timeProvider;

        public CreatePolicyCommandHandler(
            IClientRepository clientRepository,
            IBuildingReadRepository buildingReadRepository,
            IBrokerRepository brokerRepository,
            ICurrencyRepository currencyRepository,
            IFeeConfigurationReadRepository feeReadRepository,
            IRiskFactorReadRepository riskReadRepository,
            IPolicyPremiumCalculator premiumCalculator,
            IPolicyRepository policyRepository,
            IUnitOfWork unitOfWork,
            TimeProvider timeProvider)
        {
            _clientRepository = clientRepository;
            _buildingReadRepository = buildingReadRepository;
            _brokerRepository = brokerRepository;
            _currencyRepository = currencyRepository;
            _feeReadRepository = feeReadRepository;
            _riskReadRepository = riskReadRepository;
            _premiumCalculator = premiumCalculator;
            _policyRepository = policyRepository;
            _unitOfWork = unitOfWork;
            _timeProvider = timeProvider;
        }

        public async Task<Guid> Handle(CreatePolicyCommand request, CancellationToken cancellationToken)
        {
            var broker = await LoadAndValidateBroker(request.PolicyDto.BrokerId, cancellationToken);
            var currency = await LoadAndValidateCurrency(request.PolicyDto.CurrencyId, cancellationToken);

            await ValidateClientExists(request.PolicyDto.ClientId, cancellationToken);
            await ValidateBuildingOwnership(request.PolicyDto.BuildingId, request.PolicyDto.ClientId, cancellationToken);

            var geoContext = await LoadBuildingGeoContext(request.PolicyDto.BuildingId, cancellationToken);

            var calculationContext = BuildCalculationContext(geoContext);

            var fees = await LoadActiveFees(cancellationToken);
            var riskFactors = await LoadActiveRiskFactors(cancellationToken);

            var finalPremiumBase = CalculateFinalPremium(
                request.PolicyDto.BasePremium,
                calculationContext,
                fees,
                riskFactors);

            var finalPremium = ConvertToPolicyCurrency(
                finalPremiumBase,
                currency);

            var policy = CreateDraftPolicy(request, finalPremium);

            await PersistPolicy(policy, cancellationToken);

            return policy.Id;
        }

        private async Task<Broker> LoadAndValidateBroker(
            Guid brokerId,
            CancellationToken ct)
        {
            var broker = await _brokerRepository.GetByIdAsync(brokerId, ct)
                ?? throw new NotFoundException("Broker not found");

            if (!broker.IsActive)
                throw new InactiveBrokerException("Broker is inactive");

            return broker;
        }

        private async Task ValidateClientExists(Guid clientId, CancellationToken ct)
        {
            var client = await _clientRepository.GetByIdAsync(clientId, ct);

            if (client == null)
                throw new NotFoundException("Client not found");
        }

        private async Task<Currency> LoadAndValidateCurrency(Guid currencyId, CancellationToken ct)
        {
            var currency = await _currencyRepository.GetByIdAsync(currencyId, ct)
                ?? throw new NotFoundException("Currency not found");

            if (!currency.IsActive)
                throw new InactiveCurrencyException("Currency is inactive");

            return currency;
        }

        private async Task<BuildingGeoContextDto> LoadBuildingGeoContext(Guid buildingId, CancellationToken ct)
        {
            return await _buildingReadRepository
                .GetGeoContextAsync(buildingId, ct)
                ?? throw new NotFoundException("Building not found");
        }

        private static PolicyCalculationContext BuildCalculationContext(
            BuildingGeoContextDto geo)
        {
            return new PolicyCalculationContext
            {
                CityId = geo.CityId,
                CountyId = geo.CountyId,
                CountryId = geo.CountryId,
                BuildingType = geo.BuildingType,
                RiskIndicators = geo.RiskIndicators ?? new List<RiskIndicatorType>()
            };
        }

        private async Task ValidateBuildingOwnership(Guid buildingId, Guid clientId, CancellationToken ct)
        {
            var owns = await _buildingReadRepository
                .IsOwnedByClientAsync(buildingId, clientId, ct);

            if (!owns)
                throw new BuildingNotOwnedByClientException(
                    "Building does not belong to the selected client.");
        }

        private async Task<IReadOnlyList<FeeConfiguration>> LoadActiveFees(
            CancellationToken ct)
        {
            return await _feeReadRepository.GetActiveAsync(ct);
        }

        private async Task<IReadOnlyList<RiskFactorConfiguration>> LoadActiveRiskFactors(
            CancellationToken ct)
        {
            return await _riskReadRepository.GetActiveAsync(ct);
        }

        private decimal CalculateFinalPremium(
            decimal basePremium,
            PolicyCalculationContext context,
            IEnumerable<FeeConfiguration> fees,
            IEnumerable<RiskFactorConfiguration> riskFactors)
        {
            return _premiumCalculator.Calculate(
                basePremium,
                context,
                fees,
                riskFactors);
        }

        private static decimal ConvertToPolicyCurrency(decimal premiumBase, Currency currency)
        {
            return CurrencyConverter.ConvertFromBase(premiumBase, currency);
        }

        private Domain.Policies.Policy CreateDraftPolicy(
            CreatePolicyCommand request,
            decimal finalPremium)
        {
            return Domain.Policies.Policy.CreateDraft(
                clientId: request.PolicyDto.ClientId,
                buildingId: request.PolicyDto.BuildingId,
                brokerId: request.PolicyDto.BrokerId,
                currencyId: request.PolicyDto.CurrencyId,
                basePremium: request.PolicyDto.BasePremium,
                finalPremium: finalPremium,
                startDate: request.PolicyDto.StartDate,
                endDate: request.PolicyDto.EndDate,
                policyNumber: GeneratePolicyNumber(),
                now: _timeProvider.GetUtcNow().UtcDateTime
            );
        }

        private async Task PersistPolicy(
            Domain.Policies.Policy policy,
            CancellationToken ct)
        {
            await _policyRepository.AddAsync(policy, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        private static string GeneratePolicyNumber()
        {
            return $"POL-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid()}";
        }
    }

}
