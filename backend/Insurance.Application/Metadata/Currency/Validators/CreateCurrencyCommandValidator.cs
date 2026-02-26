using FluentValidation;
using Insurance.Application.Metadata.Currency.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Metadata.Currency.Validators
{
    public class CreateCurrencyCommandValidator : AbstractValidator<CreateCurrencyCommand>
    {
        public CreateCurrencyCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Currency DTO must not be null.")
                .SetValidator(new CreateCurrencyDtoValidator());
        }
    }
}
