using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Buildings.DTOs;
using Insurance.Application.Exceptions;
using Insurance.Application.Policy.Services;
using Insurance.Tests.Unit.Policy.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.Policies.Services
{
    public class AnomalyFeatureServiceTests
    {
        [Fact]
        public async Task BuildAsync_ShouldReturnExpectedAnomalyFeatureDto()
        {
            var policy = PolicyDomainTests.CreateDraftPolicy();
            var brokerId = policy.BrokerId;
            var buildingId = policy.BuildingId;
            var clientId = policy.ClientId;

            var premiumResult = new PremiumCalculationResult
            {
                PremiumInBase = 200m,
                PremiumInCurrency = 200m
            };

            var buildingContext = new BuildingAnomalyContextDto
            {
                InsuredValue = 1000m,
                SurfaceArea = 100m,
                BuildingAge = 10
            };

            var buildingRepo = new Mock<IBuildingReadRepository>();
            buildingRepo
                .Setup(r => r.GetAnomalyContextAsync(buildingId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(buildingContext);

            var policyReadRepo = new Mock<IPolicyReadRepository>();
            policyReadRepo.Setup(r => r.GetBrokerAveragePremiumAsync(brokerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(150m);
            policyReadRepo.Setup(r => r.GetBrokerGlobalAveragePremiumAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(120m);
            policyReadRepo.Setup(r => r.GetPoliciesOfClientFromLastYearAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(2);
            policyReadRepo.Setup(r => r.GetClientAverageInsuredValue(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(500m);
            policyReadRepo.Setup(r => r.GetClientGlobalAverageInsuredValue(It.IsAny<CancellationToken>()))
                .ReturnsAsync(400m);
            policyReadRepo.Setup(r => r.GetClientAveragePremiumRatioAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(0.2m);

            var service = new AnomalyFeatureService(buildingRepo.Object, policyReadRepo.Object);

            var result = await service.BuildAsync(policy, premiumResult, brokerId, buildingId, clientId, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(premiumResult.PremiumInBase, result.FinalPremiumInBase);
            Assert.Equal(buildingContext.InsuredValue, result.InsuredValue);
            Assert.Equal(buildingContext.BuildingAge, result.BuildingAge);

            Assert.Equal(0.2m, result.PremiumToInsuredValueRatio);

            Assert.Equal(50m, result.BrokerDeviationFromAverage);

            Assert.Equal(2, result.ClientPoliciesLastYear);

            Assert.Equal(2m, result.ClientInsuredValueDerivationRatio);

            Assert.Equal(1m, result.ClientPremiumRatioDerivation);

            var expectedDuration = (policy.EndDate - policy.StartDate).TotalDays;
            Assert.Equal((int)expectedDuration, result.PolicyDurationDays);

            Assert.Equal(10m, result.InsuredValuePerSquareMeter);
        }

        [Fact]
        public async Task BuildAsync_WhenBuildingNotFound_ShouldThrowNotFoundException()
        {
            var policy = PolicyDomainTests.CreateDraftPolicy();
            var brokerId = policy.BrokerId;
            var buildingId = policy.BuildingId;
            var clientId = policy.ClientId;

            var premiumResult = new PremiumCalculationResult
            {
                PremiumInBase = 100m,
                PremiumInCurrency = 100m
            };

            var buildingRepo = new Mock<IBuildingReadRepository>();
            buildingRepo
                .Setup(r => r.GetAnomalyContextAsync(buildingId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((BuildingAnomalyContextDto?)null);

            var policyReadRepo = new Mock<IPolicyReadRepository>();

            var service = new AnomalyFeatureService(buildingRepo.Object, policyReadRepo.Object);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                service.BuildAsync(policy, premiumResult, brokerId, buildingId, clientId, CancellationToken.None));
        }
    }
}
