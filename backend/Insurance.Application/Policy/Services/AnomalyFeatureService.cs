using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Exceptions;
using Insurance.Application.Policy.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Services
{
    public class AnomalyFeatureService : IAnomalyFeatureService
    {
        private readonly IBuildingReadRepository _buildingReadRepository;
        private readonly IPolicyReadRepository _policyReadRepository;

        public AnomalyFeatureService(IBuildingReadRepository buildingReadRepository, IPolicyReadRepository policyReadRepository)
        {
            _buildingReadRepository = buildingReadRepository;
            _policyReadRepository = policyReadRepository;
        }
        public async Task<AnomalyFeatureDto> BuildAsync(
    Domain.Policies.Policy policy,
    PremiumCalculationResult premiumResult,
    Guid brokerId,
    Guid buildingId,
    Guid clientId,
    CancellationToken cancellationToken)
{
    var buildingContext = await _buildingReadRepository
        .GetAnomalyContextAsync(buildingId, cancellationToken);

    if (buildingContext == null)
        throw new NotFoundException($"Building with id {buildingId} not found.");

    var brokerAveragePremium =
        await _policyReadRepository
            .GetBrokerAveragePremiumAsync(brokerId, cancellationToken);

    var globalBrokerAveragePremium =
        await _policyReadRepository
            .GetBrokerGlobalAveragePremiumAsync(cancellationToken);

    if (brokerAveragePremium == 0)
        brokerAveragePremium = globalBrokerAveragePremium;

    var clientPoliciesLastYear =
        await _policyReadRepository
            .GetPoliciesOfClientFromLastYearAsync(clientId, cancellationToken);

    var clientAverageInsuredValue =
        await _policyReadRepository
            .GetClientAverageInsuredValue(clientId, cancellationToken);

    var globalClientAverageInsuredValue =
        await _policyReadRepository
            .GetClientGlobalAverageInsuredValue(cancellationToken);

    if (clientAverageInsuredValue == 0)
        clientAverageInsuredValue = globalClientAverageInsuredValue;

    var clientAveragePremiumRatio =
        await _policyReadRepository
            .GetClientAveragePremiumRatioAsync(clientId, cancellationToken);

    decimal currentPremiumRatio =
        premiumResult.PremiumInBase / buildingContext.InsuredValue;

    decimal clientInsuredValueDeviationRatio = 1m;
    if (clientAverageInsuredValue > 0)
    {
        clientInsuredValueDeviationRatio =
            buildingContext.InsuredValue / clientAverageInsuredValue;
    }

    decimal clientPremiumRatioDeviation = 1m;
    if (clientAveragePremiumRatio > 0)
    {
        clientPremiumRatioDeviation =
            currentPremiumRatio / clientAveragePremiumRatio;
    }

    var durationDays = (policy.EndDate - policy.StartDate).TotalDays;

    return new AnomalyFeatureDto
    {
        FinalPremiumInBase = premiumResult.PremiumInBase,
        InsuredValue = buildingContext.InsuredValue,
        PremiumToInsuredValueRatio = currentPremiumRatio,
        BuildingAge = buildingContext.BuildingAge,
        BrokerDeviationFromAverage =
            premiumResult.PremiumInBase - brokerAveragePremium,
        ClientPoliciesLastYear = clientPoliciesLastYear,
        ClientInsuredValueDerivationRatio = clientInsuredValueDeviationRatio,
        ClientPremiumRatioDerivation = clientPremiumRatioDeviation,
        PolicyDurationDays = (int)durationDays,
        InsuredValuePerSquareMeter =
            buildingContext.InsuredValue / buildingContext.SurfaceArea
    };
}
    }
}
