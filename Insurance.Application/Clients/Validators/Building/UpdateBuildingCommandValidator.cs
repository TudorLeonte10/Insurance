using FluentValidation;
using Insurance.Application.Buildings.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Clients.Validators.Building
{
    public class UpdateBuildingCommandValidator : AbstractValidator<UpdateBuildingCommand>
    {
        public UpdateBuildingCommandValidator()
        {
            RuleFor(x => x.BuildingId)
                .NotEmpty()
                .WithMessage("BuildingId is required.");

            RuleFor(x => x.BuildingDto)
                .NotNull()
                .SetValidator(new UpdateBuildingDtoValidator());
        }
    }

}
