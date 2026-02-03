using FluentValidation;
using Insurance.Application.Metadata.FeeConfiguration.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Metadata.FeeConfiguration.Validators
{
    public class CreateFeeConfigurationDtoValidator : AbstractValidator<CreateFeeConfigurationDto>
    {
        public CreateFeeConfigurationDtoValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty()
               .MaximumLength(100);

            RuleFor(x => x.Type)
                .IsInEnum();

            RuleFor(x => x.Percentage)
                .NotEqual(0)
                .GreaterThanOrEqualTo(-1)
                .LessThanOrEqualTo(1)
                .WithMessage("Percentage must be between -100% and +100%");

            RuleFor(x => x.EffectiveFrom)
               .NotEmpty()
               .GreaterThanOrEqualTo(DateTime.Today)
               .WithMessage("EffectiveFrom cannot be in the past");

            RuleFor(x => x.EffectiveTo)
                .Must((dto, effectiveTo) =>
                    !effectiveTo.HasValue ||
                    effectiveTo.Value >= dto.EffectiveFrom)
                .WithMessage("EffectiveTo must be greater than or equal to EffectiveFrom");
        }
    }
}

