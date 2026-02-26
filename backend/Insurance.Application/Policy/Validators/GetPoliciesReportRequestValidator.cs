using FluentValidation;
using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Validators
{
    public class GetPoliciesReportRequestValidator : AbstractValidator<GetPoliciesReportRequestDto>
    {
        public GetPoliciesReportRequestValidator()
        {
            RuleFor(x => x.From).NotEmpty().WithMessage("'From' date is required.");
            RuleFor(x => x.To).NotEmpty().WithMessage("'To' date is required.")
                .GreaterThanOrEqualTo(x => x.From).WithMessage("'To' date must be greater than or equal to 'From' date.");

            RuleFor(x => x).Must(x => (x.To - x.From).TotalDays <= 365)
                .WithMessage("Maximum allowed interval is 1 year.");

            RuleFor(x => x.Status).IsInEnum().WithMessage("Invalid status.");
            RuleFor(x => x.Currency).Length(3).WithMessage("Value should be a valid currency code - 3 letters");
            RuleFor(x => x.BuildingType).IsInEnum().WithMessage("Invalid building type.");
        }
    }
}
