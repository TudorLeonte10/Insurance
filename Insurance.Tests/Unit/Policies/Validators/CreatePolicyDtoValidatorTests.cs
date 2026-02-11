using FluentValidation.TestHelper;
using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.Policy.Validators
{
    public class CreatePolicyDtoValidatorTests
    {
        private readonly CreatePolicyDtoValidator _validator;

        public CreatePolicyDtoValidatorTests()
        {
            _validator = new CreatePolicyDtoValidator();
        }

        [Fact]
        public void Should_Pass_For_Valid_Dto()
        {
            var dto = new CreatePolicyDto
            {
                ClientId = Guid.NewGuid(),
                BuildingId = Guid.NewGuid(),
                CurrencyId = Guid.NewGuid(),
                BasePremium = 10000,
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(365)
            };

            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Fail_When_BasePremium_Is_Not_Positive()
        {
            var dto = new CreatePolicyDto
            {
                ClientId = Guid.NewGuid(),
                BuildingId = Guid.NewGuid(),
                CurrencyId = Guid.NewGuid(),
                BasePremium = 0,
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(10)
            };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.BasePremium);
        }

        [Fact]
        public void Should_Fail_When_StartDate_Is_After_EndDate()
        {
            var dto = new CreatePolicyDto
            {
                ClientId = Guid.NewGuid(),
                BuildingId = Guid.NewGuid(),
                CurrencyId = Guid.NewGuid(),
                BasePremium = 1000,
                StartDate = DateTime.UtcNow.AddDays(10),
                EndDate = DateTime.UtcNow.AddDays(1)
            };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.StartDate);
        }

        [Fact]
        public void Should_Fail_When_StartDate_Is_In_The_Past()
        {
            var dto = new CreatePolicyDto
            {
                ClientId = Guid.NewGuid(),
                BuildingId = Guid.NewGuid(),
                CurrencyId = Guid.NewGuid(),
                BasePremium = 1000,
                StartDate = DateTime.UtcNow.AddDays(-1),
                EndDate = DateTime.UtcNow.AddDays(10)
            };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.StartDate);
        }
    }
}
