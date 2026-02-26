using FluentValidation;
using Insurance.Application.Policy.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Validators
{
    public class CreatePolicyCommandValidator : AbstractValidator<CreatePolicyCommand>
    {
        public CreatePolicyCommandValidator()
        {
            RuleFor(x => x.PolicyDto).NotNull().SetValidator(new CreatePolicyDtoValidator());
        }
    }
}
