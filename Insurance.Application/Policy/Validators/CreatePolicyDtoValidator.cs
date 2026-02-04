using FluentValidation;
using Insurance.Application.Policy.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Validators
{
    public class CreatePolicyDtoValidator
    : AbstractValidator<CreatePolicyDto>
    {
        public CreatePolicyDtoValidator()
        {
            RuleFor(x => x.ClientId)
                .NotEmpty();

            RuleFor(x => x.BuildingId)
                .NotEmpty();

            RuleFor(x => x.CurrencyId)
                .NotEmpty();

            RuleFor(x => x.BasePremium)
                .GreaterThan(0);

            RuleFor(x => x.StartDate)
                .LessThan(x => x.EndDate)
                .WithMessage("StartDate must be before EndDate.");

            RuleFor(x => x.StartDate)
                .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
                .WithMessage("StartDate cannot be in the past.");
        }
    }

}
