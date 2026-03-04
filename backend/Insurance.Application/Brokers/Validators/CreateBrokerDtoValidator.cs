using FluentValidation;
using Insurance.Application.Brokers.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Brokers.Validators
{
    public class CreateBrokerDtoValidator : AbstractValidator<CreateBrokerDto>
    {
        public CreateBrokerDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Broker name is required.")
                .MaximumLength(100).WithMessage("Broker name must not exceed 100 characters.");
            RuleFor(x => x.BrokerCode)
                .NotEmpty().WithMessage("Broker code is required.")
                .MaximumLength(50).WithMessage("Broker code must not exceed 50 characters.");
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone is required.")
                .MaximumLength(20).WithMessage("Phone must not exceed 20 characters.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email address.")
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters.");
        }
    }
}
