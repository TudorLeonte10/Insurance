using FluentValidation.TestHelper;
using Insurance.Application.Metadata.FeeConfiguration.DTOs;
using Insurance.Application.Metadata.FeeConfiguration.Validators;
using Insurance.Domain.Metadata.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.FeeConfiguration.Validators
{
    public class CreateFeeConfigurationDtoValidatorTests
    {
        [Fact]
        public void Should_Not_Have_Errors_For_Valid_Dto()
        {
            var dto = new CreateFeeConfigurationDto
            {
                Name = "Admin fee",
                Type = FeeType.AdminFee,
                Percentage = 0.05m,
                EffectiveFrom = DateTime.Today
            };

            var validator = new CreateFeeConfigurationDtoValidator();

            var result = validator.TestValidate(dto);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var dto = new CreateFeeConfigurationDto
            {
                Name = "",
                Type = FeeType.AdminFee,
                Percentage = 0.05m,
                EffectiveFrom = DateTime.Today
            };
            var validator = new CreateFeeConfigurationDtoValidator();
            var result = validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(f => f.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Percentage_Is_Out_Of_Range()
        {
            var dto = new CreateFeeConfigurationDto
            {
                Name = "Admin fee",
                Type = FeeType.AdminFee,
                Percentage = 1.5m,
                EffectiveFrom = DateTime.Today
            };
            var validator = new CreateFeeConfigurationDtoValidator();
            var result = validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(f => f.Percentage);
        }

        [Fact]
        public void Should_Have_Error_When_EffectiveFrom_Is_In_The_Past()
        {
            var dto = new CreateFeeConfigurationDto
            {
                Name = "Admin fee",
                Type = FeeType.AdminFee,
                Percentage = 0.05m,
                EffectiveFrom = DateTime.Today.AddDays(-1)
            };
            var validator = new CreateFeeConfigurationDtoValidator();
            var result = validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(f => f.EffectiveFrom);
        }
    }
}
