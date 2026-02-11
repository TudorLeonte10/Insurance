using Insurance.Application.Policy.FeeStrategies;
using Insurance.Application.Policy.RiskStrategies;
using Insurance.Application.Policy.Services;
using Insurance.Domain.Buildings;
using Insurance.Domain.Metadata.Enums;
using Insurance.Domain.RiskIndicators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.Policies.PremiumCalculation
{
    public class PremiumCalculationTests
    {
        [Fact]
        public void Calculate_NoFees_NoRisks_ShouldReturnBasePremium()
        {
            var feeCalculator = new FeeCalculator(Enumerable.Empty<IFeeStrategy>());
            var riskCalculator = new RiskFactorCalculator(Enumerable.Empty<IRiskFactorStrategy>());

            var calculator = new PolicyPremiumCalculator(
                feeCalculator,
                riskCalculator);

            var basePremium = 100m;

            var result = calculator.Calculate(
                basePremium,
                DefaultContext(),
                Enumerable.Empty<Domain.Metadata.FeeConfiguration>(),
                Enumerable.Empty<Domain.Metadata.RiskFactorConfiguration>());

            Assert.Equal(100m, result);
        }

        [Fact]
        public void Apply_WhenCityMatches_ShouldApplyAdjustment()
        {
            var cityId = Guid.NewGuid();
            var strategy = new CityRiskFactorStrategy();

            var context = new PolicyCalculationContext { CityId = cityId };

            var risk = new Domain.Metadata.RiskFactorConfiguration
            {
                Level = RiskFactorLevel.City,
                ReferenceId = cityId.ToString(),
                AdjustmentPercentage = 0.15m
            };

            var result = strategy.Apply(100m, context, risk);

            Assert.Equal(115m, result);
        }

        [Fact]
        public void Apply_WhenRiskIndicatorMatches_ShouldIncreasePremium()
        {
            var strategy = new RiskIndicatorFeeStrategy();

            var context = new PolicyCalculationContext
            {
                RiskIndicators = new[] { RiskIndicatorType.FloodRisk }
            };

            var fee = new Domain.Metadata.FeeConfiguration
            {
                Type = FeeType.RiskAdjustment,
                RiskIndicatorType = RiskIndicatorType.FloodRisk,
                Percentage = 0.20m
            };

            var result = strategy.Apply(100m, context, fee);

            Assert.Equal(120m, result);
        }

        [Fact]
        public void Calculate_MultiplePercentageAdjustments_ShouldApplyCumulatively()
        {
            var cityId = Guid.NewGuid();

            var context = DefaultContext();
            context.CityId = cityId;

            var fees = new[]
            {
                CreateFeeConfiguration(FeeType.AdminFee, 0.10m),
                CreateFeeConfiguration(FeeType.BrokerCommission, 0.05m)
            };

            var risks = new[]
            {
                CreateRiskFactorConfiguration(RiskFactorLevel.City, cityId, 0.20m)
            };

            var feeStrategies = new IFeeStrategy[]
            {
                new PercentageFeeStrategy()
            };

            var riskStrategies = new IRiskFactorStrategy[]
            {
                new CityRiskFactorStrategy()
            };

            var calculator = new PolicyPremiumCalculator(
                new FeeCalculator(feeStrategies),
                new RiskFactorCalculator(riskStrategies));

            var result = calculator.Calculate(
                100m,
                context,
                fees,
                risks);

            Assert.Equal(138.6m, result);
        }

        [Fact]
        public void Calculate_OnlyRisks_NoFees_ShouldApplyRisksOnly()
        {
            var cityId = Guid.NewGuid();
            var countyId = Guid.NewGuid();

            var context = DefaultContext();
            context.CityId = cityId;
            context.CountyId = countyId;

            var risks = new[]
            {
                CreateRiskFactorConfiguration(RiskFactorLevel.City, cityId, 0.10m),
                CreateRiskFactorConfiguration(RiskFactorLevel.County, countyId, 0.05m)
            };

            var feeStrategies = new IFeeStrategy[]
            {
                new PercentageFeeStrategy()
            };

            var riskStrategies = new IRiskFactorStrategy[]
            {
                new CityRiskFactorStrategy(),
                new CountyRiskFactorStrategy()
            };

            var calculator = new PolicyPremiumCalculator(
                new FeeCalculator(feeStrategies),
                new RiskFactorCalculator(riskStrategies));

            var result = calculator.Calculate(
                100m,
                context,
                Enumerable.Empty<Domain.Metadata.FeeConfiguration>(),
                risks);

            Assert.Equal(115.5m, result);
        }

        [Fact]
        public void Calculate_InactiveFeesAndRisks_ShouldBeIgnored()
        {
            var cityId = Guid.NewGuid();

            var context = DefaultContext();
            context.CityId = cityId;

            var fees = new[]
            {
        new Domain.Metadata.FeeConfiguration
        {
            Type = FeeType.AdminFee,
            Percentage = 0.10m,
            IsActive = false 
        }
    };

            var risks = new[]
            {
        new Domain.Metadata.RiskFactorConfiguration
        {
            Level = RiskFactorLevel.City,
            ReferenceId = cityId.ToString(),
            AdjustmentPercentage = 0.50m,
            IsActive = false
        }
    };

            var calculator = new PolicyPremiumCalculator(
                new FeeCalculator(new[] { new PercentageFeeStrategy() }),
                new RiskFactorCalculator(new[] { new CityRiskFactorStrategy() }));

            var result = calculator.Calculate(
                100m,
                context,
                fees.Where(f => f.IsActive),          
                risks.Where(r => r.IsActive));

            Assert.Equal(100m, result);
        }

        private static Domain.Metadata.FeeConfiguration CreateFeeConfiguration(
            FeeType type,
            decimal percentage,
            RiskIndicatorType? riskIndicatorType = null)
        {
            return new Domain.Metadata.FeeConfiguration
            {
                Id = Guid.NewGuid(),
                Name = $"{type} Test",
                Type = type,
                Percentage = percentage,
                RiskIndicatorType = riskIndicatorType,
                IsActive = true,
                EffectiveFrom = DateTime.UtcNow.AddDays(-10),
                EffectiveTo = DateTime.UtcNow.AddDays(10)
            };
        }

        private static Domain.Metadata.RiskFactorConfiguration CreateRiskFactorConfiguration(
            RiskFactorLevel level,
            Guid referenceId,
            decimal adjustmentPercentage)
        {
            return new Domain.Metadata.RiskFactorConfiguration
            {
                Id = Guid.NewGuid(),
                Level = level,
                ReferenceId = referenceId.ToString(),
                AdjustmentPercentage = adjustmentPercentage,
                IsActive = true
            };
        }

        private static PolicyCalculationContext DefaultContext() =>
    new PolicyCalculationContext
    {
        CityId = Guid.NewGuid(),
        CountyId = Guid.NewGuid(),
        CountryId = Guid.NewGuid(),
        BuildingType = BuildingType.Residential,
        RiskIndicators = new List<RiskIndicatorType>()
    };
      
    }
}
