using FluentValidation;
using Insurance.Application.Clients.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Clients.Validators.Client
{
    public class CreateClientDtoValidator
    : AbstractValidator<CreateClientDto>
    {
        public CreateClientDtoValidator()
        {
            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("Client type is required and must be a valid enum value.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200)
                .WithMessage("Client name is required and should not exceed 200 characters.");

            RuleFor(x => x.IdentificationNumber)
                .NotEmpty()
                .MaximumLength(20)
                .WithMessage("Identification number is required and should not exceed 20 characters.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("A valid email address is required.");


            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("Phone number is required.");
        }
    }
}
