using FluentValidation;
using Insurance.Application.Clients.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Clients.Validators
{
    public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
    {
        public CreateClientCommandValidator()
        {
            RuleFor(c => c.Dto.Name).NotEmpty().WithMessage("Client name is required.")
                .MaximumLength(200).WithMessage("Client name must not exceed 200 characters.");

            RuleFor(c => c.Dto.IdentificationNumber).NotEmpty().WithMessage("Identification number is required.")
                .MaximumLength(20).WithMessage("Identification number must not exceed 20 characters.");

            RuleFor(c => c.Dto.Type).IsInEnum().WithMessage("Invalid client type.");

            RuleFor(c => c.Dto.ContactInfo).NotEmpty().WithMessage("Contact information is required.");
        }
    }
}
