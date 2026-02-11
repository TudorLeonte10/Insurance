using FluentValidation;
using Insurance.Application.Brokers.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Brokers.Validators
{
    public class UpdateBrokerCommandValidator : AbstractValidator<UpdateBrokerCommand>
    {
        public UpdateBrokerCommandValidator()
        {
            RuleFor(x => x.brokerDto).NotNull().SetValidator(x => new UpdateBrokerDtoValidator());
            RuleFor(x => x.brokerId).NotEqual(Guid.Empty);
        }
    }
}
