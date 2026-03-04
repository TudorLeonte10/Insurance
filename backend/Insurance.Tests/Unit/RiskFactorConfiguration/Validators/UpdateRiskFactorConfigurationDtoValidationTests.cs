using Insurance.Application.Metadata.RiskFactors.DTOs;
using Insurance.Application.Metadata.RiskFactors.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.RiskFactorConfiguration.Validators
{
    public class UpdateRiskFactorConfigurationDtoValidationTests
    {
        [Fact]
        public void Given_InvalidDto_Should_FailValidation()
        {
            var dto = new UpdateRiskFactorConfigurationDto
            {
                Level = Domain.Metadata.Enums.RiskFactorLevel.BuildingType,
                ReferenceId = "",
                AdjustmentPercentage = -10,
                IsActive = true
            };
            var validator = new UpdateRiskFactorConfigurationDtoValidator();

            var result = validator.Validate(dto);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void Given_ValidDto_Should_PassValidation()
        {
            var dto = new UpdateRiskFactorConfigurationDto
            {
                Level = Domain.Metadata.Enums.RiskFactorLevel.BuildingType,
                ReferenceId = "Residential",
                AdjustmentPercentage = 15,
                IsActive = true
            };
            var validator = new UpdateRiskFactorConfigurationDtoValidator();
            var result = validator.Validate(dto);
            Assert.True(result.IsValid);
        }
    }
}
