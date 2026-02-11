using FluentValidation.TestHelper;
using Insurance.Application.Metadata.Currency.DTOs;
using Insurance.Application.Metadata.Currency.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.Currency.Validators
{
    public class UpdateCurrencyDtoValidatorTests
    {
        private readonly UpdateCurrencyDtoValidator _validator;

        public UpdateCurrencyDtoValidatorTests()
        {
            _validator = new UpdateCurrencyDtoValidator();
        }

        [Fact]
        public void Should_Not_Have_Errors_For_Valid_Dto()
        {
            var dto = new UpdateCurrencyDto
            {
                Code = "USD",
                Name = "US Dollar",
                ExchangeRateToBase = 4.6m
            };

            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Have_Error_For_Invalid_Code()
        {
            var dto = new UpdateCurrencyDto
            {
                Code = "us",
                Name = "Dollar",
                ExchangeRateToBase = 4.6m
            };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Code);
        }
    }
}
