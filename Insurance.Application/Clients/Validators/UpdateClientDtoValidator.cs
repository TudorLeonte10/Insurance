using FluentValidation;
using Insurance.Application.Clients.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Clients.Validators
{
    public class UpdateClientDtoValidator
    : AbstractValidator<UpdateClientDto>
    {
        public UpdateClientDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(40)
                .WithMessage("Name is required and should not exceed 40 characters.");
            RuleFor(x => x.Email)
                .EmailAddress()
                .NotEmpty()
                .WithMessage("A valid email address is required.");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("Phone number is required.");
        }
    }
}
