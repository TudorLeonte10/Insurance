using FluentValidation;
using Insurance.Application.Policy.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Validators
{
    public class GetPolicyReportQueryValidator : AbstractValidator<GetPoliciesReportQuery>
    {
        public GetPolicyReportQueryValidator()
        {
            RuleFor(x => x.Dto).SetValidator(x => new GetPoliciesReportRequestValidator());
            RuleFor(x => x.GroupingType).IsInEnum().WithMessage("Invalid grouping type.");
        }
    }
}
