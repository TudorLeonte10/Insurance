using FluentValidation;
using Insurance.Application.Buildings.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Buildings.Validators
{
    public class CreateBuildingCommandValidator : AbstractValidator<CreateBuildingCommand>
    {
        public CreateBuildingCommandValidator()
        {
            RuleFor(x => x.ClientId)
                .NotEmpty()
                .WithMessage("ClientId is required.");

            RuleFor(x => x.BuildingDto)
                .NotNull()
                .SetValidator(new CreateBuildingDtoValidator());
        }
    }
}
