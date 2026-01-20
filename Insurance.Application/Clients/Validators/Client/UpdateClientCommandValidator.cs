using FluentValidation;
using Insurance.Application.Clients.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Clients.Validators.Client
{
    public class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
    {
        public UpdateClientCommandValidator()
        {
            RuleFor(x => x.Dto)
            .NotNull()
            .SetValidator(new UpdateClientDtoValidator());
        }
    }

}
