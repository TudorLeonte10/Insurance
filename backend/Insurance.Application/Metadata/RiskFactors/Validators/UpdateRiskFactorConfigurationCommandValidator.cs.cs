using FluentValidation;
using Insurance.Application.Metadata.RiskFactors.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Metadata.RiskFactors.Validators
{
    public class UpdateRiskFactorConfigurationCommandValidator : AbstractValidator<UpdateRiskFactorConfigurationCommand>
    {
        public UpdateRiskFactorConfigurationCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().SetValidator(new UpdateRiskFactorConfigurationDtoValidator());
            RuleFor(x => x.Id).NotEmpty().WithMessage("Risk factor configuration ID must not be empty.");
        }
    }
}
