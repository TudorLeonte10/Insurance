using FluentValidation;
using Insurance.Application.Brokers.Commands;
using Insurance.Application.Brokers.Validators;
using Insurance.Application.Metadata.Currency.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Metadata.Currency.Validators
{
    public class UpdateCurrencyCommandValidator : AbstractValidator<UpdateCurrencyCommand>
    {
        public UpdateCurrencyCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Currency ID must not be empty.");
            RuleFor(x => x.Dto).NotNull().WithMessage("Currency DTO must not be null.")
                .SetValidator(new UpdateCurrencyDtoValidator());
        }
    }
}
