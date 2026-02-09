using FluentValidation;
using Insurance.Application.Clients.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Clients.Validators
{
    public class CreateClientCommandValidator
    : AbstractValidator<CreateClientCommand>
    {
        public CreateClientCommandValidator()
        {
            RuleFor(x => x.Dto)
                .NotNull()
                .SetValidator(new CreateClientDtoValidator());
        }
    }
}
