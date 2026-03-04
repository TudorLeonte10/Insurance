using FluentValidation;
using Insurance.Application.Brokers.Commands;
using Insurance.Application.Clients.Commands;
using Insurance.Application.Clients.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Brokers.Validators
{
    public class CreateBrokerCommandValidator
    : AbstractValidator<CreateBrokerCommand>
    {
        public CreateBrokerCommandValidator()
        {
            RuleFor(x => x.Dto)
                .NotNull()
                .SetValidator(new CreateBrokerDtoValidator());
        }
    }
}
