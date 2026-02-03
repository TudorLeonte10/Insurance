using FluentValidation;
using Insurance.Application.FeeConfiguration.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.FeeConfiguration.Validators
{
    public class CreateFeeConfigurationCommandValidator : AbstractValidator<CreateFeeConfigurationCommand>
    {
        public CreateFeeConfigurationCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().SetValidator(new CreateFeeConfigurationDtoValidator());
        }
    }
}
