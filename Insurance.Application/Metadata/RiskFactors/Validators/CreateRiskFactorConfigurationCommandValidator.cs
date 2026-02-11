using FluentValidation;
using Insurance.Application.Metadata.RiskFactors.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Metadata.RiskFactors.Validators
{
    public class CreateRiskFactorConfigurationCommandValidator : AbstractValidator<CreateRiskFactorConfigurationCommand>
    {
        public CreateRiskFactorConfigurationCommandValidator()
        {
            RuleFor(x => x.RiskFactorConfigurationDto)
                .NotNull()
                .SetValidator(new CreateRiskFactorConfigurationDtoValidator());
        }
    }
}
