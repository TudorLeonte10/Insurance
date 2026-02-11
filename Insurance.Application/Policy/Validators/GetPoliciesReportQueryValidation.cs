using FluentValidation;
using Insurance.Application.Policy.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Validators
{
    public class GetPoliciesReportQueryValidation : AbstractValidator<GetPoliciesReportQuery>
    {
        public GetPoliciesReportQueryValidation()
        {
            When(x => x.From.HasValue && x.To.HasValue, () =>
            {
                RuleFor(x => x.To).GreaterThanOrEqualTo(x => x.From).WithMessage("The 'To' date must be greater than or equal to the 'From' date.");
            });
            RuleFor(x => x.GroupingType).IsInEnum().WithMessage("Invalid grouping type.");
            RuleFor(x => x.Status).IsInEnum().WithMessage("Invalid status.");
            RuleFor(x => x.Currency).Length(3).WithMessage("Value should be a valid currency code - 3 letters");
            RuleFor(x => x.BuildingType).IsInEnum().WithMessage("Invalid building type.");
        }
    }
}
