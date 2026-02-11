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
    public class BuildingTypeRiskStrategyTests
    {
        private readonly BuildingTypeRiskStrategy _strategy;

        public BuildingTypeRiskStrategyTests()
        {
            _strategy = new BuildingTypeRiskStrategy();
        }

        [Fact]
        public void CanHandle_WithBuildingTypeLevel_ShouldReturnTrue()
        {
            var result = _strategy.CanHandle(RiskFactorLevel.BuildingType);

            Assert.True(result);
        }

        [Theory]
        [InlineData(RiskFactorLevel.City)]
        [InlineData(RiskFactorLevel.County)]
        [InlineData(RiskFactorLevel.Country)]
        public void CanHandle_WithOtherLevels_ShouldReturnFalse(RiskFactorLevel level)
        {
            var result = _strategy.CanHandle(level);

            Assert.False(result);
        }


        [Fact]
        public void Apply_WhenBuildingTypeDoesNotMatch_ShouldReturnOriginalPremium()
        {
            var basePremium = 1000m;

            var context = new PolicyCalculationContext
            {
                BuildingType = BuildingType.Residential,
                CityId = Guid.NewGuid(),
                CountyId = Guid.NewGuid(),
                CountryId = Guid.NewGuid(),
                RiskIndicators = new List<RiskIndicatorType>()
            };

            var risk = new Domain.Metadata.RiskFactorConfiguration
            {
                Id = Guid.NewGuid(),
                Level = RiskFactorLevel.BuildingType,
                ReferenceId = "Commercial", 
                AdjustmentPercentage = 0.15m,
                IsActive = true
            };

            var result = _strategy.Apply(basePremium, context, risk);

            Assert.Equal(basePremium, result); 
        }
    }
}
