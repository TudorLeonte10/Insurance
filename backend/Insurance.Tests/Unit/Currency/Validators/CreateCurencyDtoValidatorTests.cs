using FluentValidation.TestHelper;
using Insurance.Application.Metadata.Currency.DTOs;
using Insurance.Application.Metadata.Currency.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.Currency.Validators
{
    public class CreateCurrencyDtoValidatorTests
    {
        private readonly CreateCurrencyDtoValidator _validator;

        public CreateCurrencyDtoValidatorTests()
        {
            _validator = new CreateCurrencyDtoValidator();
        }

        [Fact]
        public void Should_Not_Have_Errors_For_Valid_Dto()
        {
            var dto = new CreateCurrencyDto
            {
                Code = "EUR",
                Name = "Euro",
                ExchangeRateToBase = 4.95m
            };

            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("")]
        [InlineData("EU")]
        [InlineData("EURO")]
        [InlineData("eur")]
        public void Should_Have_Error_For_Invalid_Code(string code)
        {
            var dto = new CreateCurrencyDto
            {
                Code = code,
                Name = "Euro",
                ExchangeRateToBase = 4.95m
            };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Code);
        }

        [Fact]
        public void Should_Have_Error_When_ExchangeRate_Is_Not_Positive()
        {
            var dto = new CreateCurrencyDto
            {
                Code = "EUR",
                Name = "Euro",
                ExchangeRateToBase = 0
            };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.ExchangeRateToBase);
        }
    }
}
