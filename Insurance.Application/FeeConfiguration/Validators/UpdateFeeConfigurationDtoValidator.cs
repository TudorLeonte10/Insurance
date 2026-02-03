using FluentValidation;
using Insurance.Application.FeeConfiguration.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.FeeConfiguration.Validators
{
    public class UpdateFeeConfigurationDtoValidator : AbstractValidator<UpdateFeeConfigurationDto>
    {
        public UpdateFeeConfigurationDtoValidator()
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
                .NotEmpty();

            RuleFor(x => x.EffectiveTo)
                .Must((dto, effectiveTo) =>
                    !effectiveTo.HasValue ||
                    effectiveTo.Value >= dto.EffectiveFrom)
                .WithMessage("EffectiveTo must be greater than or equal to EffectiveFrom");
        }
    }
}
