using FluentValidation;
using Insurance.Application.Metadata.Currency.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Metadata.Currency.Validators
{
    public class UpdateCurrencyDtoValidator : AbstractValidator<UpdateCurrencyDto>
    {
        public UpdateCurrencyDtoValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .Length(3)
                .Matches("^[A-Z]{3}$")
                .WithMessage("Currency code must be exactly 3 uppercase letters.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.ExchangeRateToBase)
                .GreaterThan(0)
                .WithMessage("ExchangeRateToBase must be greater than 0.");
        }
    }
}
