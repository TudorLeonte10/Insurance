using FluentValidation;
using Insurance.Application.Buildings.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Buildings.Validators
{
    public class UpdateBuildingDtoValidator : AbstractValidator<UpdateBuildingDto>
    {
        public UpdateBuildingDtoValidator()
        {
            RuleFor(x => x.Street)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Number)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.ConstructionYear)
                .InclusiveBetween(1800, DateTime.UtcNow.Year);

            RuleFor(x => x.SurfaceArea)
                .GreaterThan(0);

            RuleFor(x => x.NumberOfFloors)
                .GreaterThan(0);

            RuleFor(x => x.InsuredValue)
                .GreaterThan(0);

            RuleForEach(x => x.RiskIndicators)
                .IsInEnum();
        }
    }
}
