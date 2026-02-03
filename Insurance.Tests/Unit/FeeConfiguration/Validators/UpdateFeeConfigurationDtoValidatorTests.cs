using Insurance.Application.FeeConfiguration.DTOs;
using Insurance.Application.FeeConfiguration.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.FeeConfiguration.Validators
{
    public class UpdateFeeConfigurationDtoValidatorTests
    {
        [Fact]
        public void Should_Not_Have_Errors_For_Valid_Dto()
        {
            var dto = new UpdateFeeConfigurationDto
            {
                Name = "Standard Fee",
                Type = Domain.Metadata.Enums.FeeType.ConfigurationFee,
                Percentage = 0.15m,
                EffectiveFrom = DateTime.UtcNow,
                EffectiveTo = DateTime.UtcNow.AddYears(1),
                IsActive = true
            };
            var validator = new UpdateFeeConfigurationDtoValidator();
            var result = validator.Validate(dto);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var dto = new UpdateFeeConfigurationDto
            {
                Name = "",
                Percentage = 0.15m,
                EffectiveFrom = DateTime.UtcNow,
                EffectiveTo = DateTime.UtcNow.AddYears(1),
                IsActive = true
            };
            var validator = new UpdateFeeConfigurationDtoValidator();
            var result = validator.Validate(dto);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Name");
        }

        [Fact]
        public void Should_Have_Error_When_Percentage_Is_Out_Of_Range()
        {
            var dto = new UpdateFeeConfigurationDto
            {
                Name = "Standard Fee",
                Percentage = 1.5m,
                EffectiveFrom = DateTime.UtcNow,
                EffectiveTo = DateTime.UtcNow.AddYears(1),
                IsActive = true
            };
            var validator = new UpdateFeeConfigurationDtoValidator();
            var result = validator.Validate(dto);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Percentage");
        }

        [Fact]
        public void Should_Have_Error_When_EffectiveTo_Is_Before_EffectiveFrom()
        {
            var dto = new UpdateFeeConfigurationDto
            {
                Name = "Standard Fee",
                Percentage = 0.15m,
                EffectiveFrom = DateTime.UtcNow,
                EffectiveTo = DateTime.UtcNow.AddDays(-1),
                IsActive = true
            };
            var validator = new UpdateFeeConfigurationDtoValidator();
            var result = validator.Validate(dto);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "EffectiveTo");
        }
    }
}
