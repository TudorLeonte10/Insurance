using Insurance.Application.Policy.RiskStrategies;
using Insurance.Application.Policy.Services;
using Insurance.Domain.Buildings;
using Insurance.Domain.Metadata.Enums;
using Insurance.Domain.RiskIndicators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.Policies.Strategies
{
    public class CountryRiskFactorStrategyTests
    {
        private readonly CountryRiskFactorStrategy _strategy;

        public CountryRiskFactorStrategyTests()
        {
            _strategy = new CountryRiskFactorStrategy();
        }

        [Fact]
        public void CanHandle_WithCountryLevel_ShouldReturnTrue()
        {
            var result = _strategy.CanHandle(RiskFactorLevel.Country);

            Assert.True(result);
        }

        [Theory]
        [InlineData(RiskFactorLevel.City)]
        [InlineData(RiskFactorLevel.County)]
        [InlineData(RiskFactorLevel.BuildingType)]
        public void CanHandle_WithOtherLevels_ShouldReturnFalse(RiskFactorLevel level)
        {
            var result = _strategy.CanHandle(level);
            Assert.False(result);
        }

        [Fact]
        public void Apply_WhenCountryIdMatches_ShouldApplyAdjustment()
        {
            var basePremium = 1000m;
            var adjustmentPercentage = 0.12m; 
            var countryId = Guid.NewGuid();

            var context = new PolicyCalculationContext
            {
                CountryId = countryId,
                CountyId = Guid.NewGuid(),
                CityId = Guid.NewGuid(),
                BuildingType = BuildingType.Residential,
                RiskIndicators = new List<RiskIndicatorType>()
            };

            var risk = new Domain.Metadata.RiskFactorConfiguration
            {
                Id = Guid.NewGuid(),
                Level = RiskFactorLevel.Country,
                ReferenceId = countryId.ToString(), 
                AdjustmentPercentage = adjustmentPercentage,
                IsActive = true
            };

            var result = _strategy.Apply(basePremium, context, risk);

            Assert.Equal(1120m, result);
        }

        [Fact]
        public void Apply_WhenCountryIdDoesNotMatch_ShouldReturnOriginalPremium()
        {
            var basePremium = 1000m;
            var countryId = Guid.NewGuid();
            var differentCountryId = Guid.NewGuid();

            var context = new PolicyCalculationContext
            {
                CountryId = countryId,
                CountyId = Guid.NewGuid(),
                CityId = Guid.NewGuid(),
                BuildingType = BuildingType.Residential,
                RiskIndicators = new List<RiskIndicatorType>()
            };

            var risk = new Domain.Metadata.RiskFactorConfiguration
            {
                Id = Guid.NewGuid(),
                Level = RiskFactorLevel.Country,
                ReferenceId = differentCountryId.ToString(), 
                AdjustmentPercentage = 0.12m,
                IsActive = true
            };

            var result = _strategy.Apply(basePremium, context, risk);

            Assert.Equal(basePremium, result); 
        }

        [Theory]
        [InlineData(0.05)]
        [InlineData(0.10)]
        [InlineData(0.15)]
        [InlineData(0.25)]
        public void Apply_WithVariousAdjustmentPercentages_ShouldApplyCorrectly(decimal adjustmentPercentage)
        {
            var basePremium = 1000m;
            var countryId = Guid.NewGuid();

            var context = new PolicyCalculationContext
            {
                CountryId = countryId,
                CountyId = Guid.NewGuid(),
                CityId = Guid.NewGuid(),
                BuildingType = BuildingType.Residential,
                RiskIndicators = new List<RiskIndicatorType>()
            };

            var risk = new Domain.Metadata.RiskFactorConfiguration
            {
                Id = Guid.NewGuid(),
                Level = RiskFactorLevel.Country,
                ReferenceId = countryId.ToString(),
                AdjustmentPercentage = adjustmentPercentage,
                IsActive = true
            };

            var result = _strategy.Apply(basePremium, context, risk);

            var expected = basePremium * (1 + adjustmentPercentage);
            Assert.Equal(expected, result);
        }
    }
}
