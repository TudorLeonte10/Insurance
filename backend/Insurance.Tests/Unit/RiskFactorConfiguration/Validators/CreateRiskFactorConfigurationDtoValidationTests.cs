using Insurance.Application.Metadata.RiskFactors.DTOs;
using Insurance.Application.Metadata.RiskFactors.Validators;
using Insurance.Domain.Metadata.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.RiskFactorConfiguration.Validators
{
    public class CreateRiskFactorConfigurationDtoValidationTests
    {
        [Fact]
        public void Should_Fail_When_Adjustment_Out_Of_Range()
        {
            var validator = new CreateRiskFactorConfigurationDtoValidator();

            var dto = new CreateRiskFactorConfigurationDto
            {
                Level = RiskFactorLevel.Country,
                ReferenceId = "TEST",
                AdjustmentPercentage = 200
            };

            var result = validator.Validate(dto);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void Should_Pass_When_Data_Is_Valid()
        {
            var validator = new CreateRiskFactorConfigurationDtoValidator();
            var dto = new CreateRiskFactorConfigurationDto
            {
                Level = RiskFactorLevel.Country,
                ReferenceId = "TEST",
                AdjustmentPercentage = 15
            };
            var result = validator.Validate(dto);
            Assert.True(result.IsValid);
        }
    }
}
