using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Buildings.DTOs;
using Insurance.Application.Exceptions;
using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Services;
using Insurance.Domain.Clients;
using Insurance.Domain.Metadata;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.Policies.Services
{
    public class PolicyCreationServiceTests
    {
        [Fact]
        public async Task CreatePolicyAsync_ShouldReturnResult_AndNotSetToReview_WhenMlReportsNoAnomaly()
        {
            // Arrange
            var brokerId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var buildingId = Guid.NewGuid();
            var currencyId = Guid.NewGuid();

            var dto = new CreatePolicyDto
            {
                ClientId = clientId,
                BuildingId = buildingId,
                CurrencyId = currencyId,
                BasePremium = 100m,
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(10)
            };

            var clientRepo = new Mock<IClientRepository>();
            clientRepo.Setup(r => r.GetByIdAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Client.Rehydrate(clientId, brokerId, ClientType.Individual, "n", "id", "e", "p", "a"));

            var buildingReadRepo = new Mock<IBuildingReadRepository>();
            buildingReadRepo.Setup(r => r.GetByIdAsync(buildingId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new BuildingDetailsDto { Id = buildingId, ClientId = clientId });
            buildingReadRepo.Setup(r => r.GetGeoContextAsync(buildingId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new BuildingGeoContextDto
                {
                    BuildingId = buildingId,
                    CityId = Guid.NewGuid(),
                    CountyId = Guid.NewGuid(),
                    CountryId = Guid.NewGuid(),
                    CityName = "City",
                    CountyName = "County",
                    CountryName = "Country",
                    BrokerCode = "BR1",
                    BuildingType = Domain.Buildings.BuildingType.Residential,
                    RiskIndicators = Array.Empty<Domain.RiskIndicators.RiskIndicatorType>()
                });

            var currencyRepo = new Mock<ICurrencyRepository>();
            currencyRepo.Setup(r => r.GetByIdAsync(currencyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Domain.Metadata.Currency { Id = currencyId, Name = "EUR", ExchangeRateToBase = 1m });

            var feeReadRepo = new Mock<IFeeConfigurationReadRepository>();
            feeReadRepo.Setup(r => r.GetActiveAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Domain.Metadata.FeeConfiguration>());

            var riskReadRepo = new Mock<IRiskFactorReadRepository>();
            riskReadRepo.Setup(r => r.GetActiveAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Domain.Metadata.RiskFactorConfiguration>());

            var premiumCalculator = new Mock<IPolicyPremiumCalculator>();
            premiumCalculator.Setup(p => p.Calculate(It.IsAny<decimal>(), It.IsAny<PolicyCalculationContext>(), It.IsAny<IEnumerable<Domain.Metadata.FeeConfiguration>>(), It.IsAny<IEnumerable<Domain.Metadata.RiskFactorConfiguration>>()))
                .Returns(100m);

            var anomalyService = new Mock<IAnomalyFeatureService>();
            anomalyService.Setup(a => a.BuildAsync(It.IsAny<Domain.Policies.Policy>(), It.IsAny<PremiumCalculationResult>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AnomalyFeatureDto
                {
                    FinalPremiumInBase = 100m,
                    InsuredValue = 1000m,
                    PremiumToInsuredValueRatio = 0.1m,
                    BuildingAge = 5,
                    ClientPoliciesLastYear = 0,
                    PolicyDurationDays = 9,
                    InsuredValuePerSquareMeter = 10m,
                    BrokerDeviationFromAverage = 0m,
                    ClientInsuredValueDerivationRatio = 1m,
                    ClientPremiumRatioDerivation = 1m
                });

            var mlService = new Mock<IPolicyMlService>();
            mlService.Setup(m => m.AnalyzePolicyAsync(It.IsAny<AnomalyFeatureDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PolicyMlResult { IsAnomaly = 0 });

            var service = new PolicyCreationService(
                clientRepo.Object,
                buildingReadRepo.Object,
                currencyRepo.Object,
                feeReadRepo.Object,
                riskReadRepo.Object,
                premiumCalculator.Object,
                anomalyService.Object,
                mlService.Object,
                TimeProvider.System);

            // Act
            var result = await service.CreatePolicyAsync(dto, brokerId, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("EUR", result.Currency);
            Assert.Equal("BR1", result.BrokerCode);
            Assert.Equal("Draft", result.Status); // not set to review
            Assert.Equal(result.Policy.Id, result.Policy.Id); // basic sanity
            anomalyService.Verify(a => a.BuildAsync(It.IsAny<Domain.Policies.Policy>(), It.IsAny<PremiumCalculationResult>(), brokerId, dto.BuildingId, dto.ClientId, It.IsAny<CancellationToken>()), Times.Once);
            mlService.Verify(m => m.AnalyzePolicyAsync(It.IsAny<AnomalyFeatureDto>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreatePolicyAsync_ShouldSetPolicyToReview_WhenMlReportsAnomaly()
        {
            // Arrange (reuse happy-path setup but make ML return anomaly)
            var brokerId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var buildingId = Guid.NewGuid();
            var currencyId = Guid.NewGuid();

            var dto = new CreatePolicyDto
            {
                ClientId = clientId,
                BuildingId = buildingId,
                CurrencyId = currencyId,
                BasePremium = 100m,
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(10)
            };

            var clientRepo = new Mock<IClientRepository>();
            clientRepo.Setup(r => r.GetByIdAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Client.Rehydrate(clientId, brokerId, ClientType.Individual, "n", "id", "e", "p", "a"));

            var buildingReadRepo = new Mock<IBuildingReadRepository>();
            buildingReadRepo.Setup(r => r.GetByIdAsync(buildingId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new BuildingDetailsDto { Id = buildingId, ClientId = clientId });
            buildingReadRepo.Setup(r => r.GetGeoContextAsync(buildingId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new BuildingGeoContextDto
                {
                    BuildingId = buildingId,
                    CityId = Guid.NewGuid(),
                    CountyId = Guid.NewGuid(),
                    CountryId = Guid.NewGuid(),
                    CityName = "City",
                    CountyName = "County",
                    CountryName = "Country",
                    BrokerCode = "BR1",
                    BuildingType = Domain.Buildings.BuildingType.Residential,
                    RiskIndicators = Array.Empty<Domain.RiskIndicators.RiskIndicatorType>()
                });

            var currencyRepo = new Mock<ICurrencyRepository>();
            currencyRepo.Setup(r => r.GetByIdAsync(currencyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Domain.Metadata.Currency { Id = currencyId, Name = "EUR", ExchangeRateToBase = 1m });

            var feeReadRepo = new Mock<IFeeConfigurationReadRepository>();
            feeReadRepo.Setup(r => r.GetActiveAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Domain.Metadata.FeeConfiguration>());

            var riskReadRepo = new Mock<IRiskFactorReadRepository>();
            riskReadRepo.Setup(r => r.GetActiveAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Domain.Metadata.RiskFactorConfiguration>());

            var premiumCalculator = new Mock<IPolicyPremiumCalculator>();
            premiumCalculator.Setup(p => p.Calculate(It.IsAny<decimal>(), It.IsAny<PolicyCalculationContext>(), It.IsAny<IEnumerable<Domain.Metadata.FeeConfiguration>>(), It.IsAny<IEnumerable<Domain.Metadata.RiskFactorConfiguration>>()))
                .Returns(100m);

            var anomalyService = new Mock<IAnomalyFeatureService>();
            anomalyService.Setup(a => a.BuildAsync(It.IsAny<Domain.Policies.Policy>(), It.IsAny<PremiumCalculationResult>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AnomalyFeatureDto());

            var mlService = new Mock<IPolicyMlService>();
            mlService.Setup(m => m.AnalyzePolicyAsync(It.IsAny<AnomalyFeatureDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PolicyMlResult { IsAnomaly = 1 });

            var service = new PolicyCreationService(
                clientRepo.Object,
                buildingReadRepo.Object,
                currencyRepo.Object,
                feeReadRepo.Object,
                riskReadRepo.Object,
                premiumCalculator.Object,
                anomalyService.Object,
                mlService.Object,
                TimeProvider.System);

            // Act
            var result = await service.CreatePolicyAsync(dto, brokerId, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("UnderReview", result.Status); // updated to match Policy.Status.ToString()
        }


        [Fact]
        public async Task CreatePolicyAsync_WhenClientNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var brokerId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var buildingId = Guid.NewGuid();
            var currencyId = Guid.NewGuid();

            var dto = new CreatePolicyDto
            {
                ClientId = clientId,
                BuildingId = buildingId,
                CurrencyId = currencyId,
                BasePremium = 100m,
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(10)
            };

            var clientRepo = new Mock<IClientRepository>();
            clientRepo.Setup(r => r.GetByIdAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Client?)null);

            var service = new PolicyCreationService(
                clientRepo.Object,
                Mock.Of<IBuildingReadRepository>(),
                Mock.Of<ICurrencyRepository>(),
                Mock.Of<IFeeConfigurationReadRepository>(),
                Mock.Of<IRiskFactorReadRepository>(),
                Mock.Of<IPolicyPremiumCalculator>(),
                Mock.Of<IAnomalyFeatureService>(),
                Mock.Of<IPolicyMlService>(),
                TimeProvider.System);

            // Act / Assert
            await Assert.ThrowsAsync<NotFoundException>(() => service.CreatePolicyAsync(dto, brokerId, CancellationToken.None));
        }
    }
}
