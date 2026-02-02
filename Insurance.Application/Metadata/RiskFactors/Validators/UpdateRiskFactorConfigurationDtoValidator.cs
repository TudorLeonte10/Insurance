using FluentValidation;
using Insurance.Application.Metadata.RiskFactors.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Metadata.RiskFactors.Validators
{
    public class UpdateRiskFactorConfigurationDtoValidator : AbstractValidator<UpdateRiskFactorConfigurationDto>
    {
        public UpdateRiskFactorConfigurationDtoValidator()
        {
            RuleFor(x => x.ReferenceId)
                .NotEmpty().WithMessage("ReferenceId is required.");
            RuleFor(x => x.AdjustmentPercentage)
                .InclusiveBetween(0, 100).WithMessage("AdjustmentPercentage must be between 0 and 100.");
            RuleFor(x => x.Level).NotNull().IsInEnum().WithMessage("Level is required.");
        }
    }
}
