using FluentValidation;
using Insurance.Application.Brokers.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Brokers.Validators
{
    public class UpdateBrokerDtoValidator : AbstractValidator<UpdateBrokerDto>
    {
        public UpdateBrokerDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Broker name is required.")
                .MaximumLength(100).WithMessage("Broker name must not exceed 100 characters.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Broker email is required.")
                .EmailAddress().WithMessage("Broker email must be a valid email address.")
                .MaximumLength(100).WithMessage("Broker email must not exceed 100 characters.");
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Broker phone number is required.")
                .MaximumLength(15).WithMessage("Broker phone number must not exceed 15 characters.");
        }
    }
}
