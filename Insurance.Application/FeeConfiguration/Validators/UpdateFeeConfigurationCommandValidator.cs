using FluentValidation;
using Insurance.Application.FeeConfiguration.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.FeeConfiguration.Validators
{
    public class UpdateFeeConfigurationCommandValidator : AbstractValidator<UpdateFeeConfigurationCommand>
    {
        public UpdateFeeConfigurationCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().SetValidator(new UpdateFeeConfigurationDtoValidator());
        }
    }
}
