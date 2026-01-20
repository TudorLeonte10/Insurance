using FluentValidation;
using Insurance.Application.Buildings.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Clients.Validators.Building
{
    public class CreateBuildingDtoValidator : AbstractValidator<CreateBuildingDto>
    {
        public CreateBuildingDtoValidator()
        {
            RuleFor(x => x.Street)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Number)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.CityId)
                .NotEmpty()
                .WithMessage("City is required.");

            RuleFor(x => x.ConstructionYear)
                .InclusiveBetween(1800, DateTime.UtcNow.Year)
                .WithMessage("Invalid construction year.");

            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("Invalid building type.");

            RuleFor(x => x.SurfaceArea)
                .GreaterThan(0);

            RuleFor(x => x.NumberOfFloors)
                .GreaterThan(0);

            RuleFor(x => x.InsuredValue)
                .GreaterThan(0);

            RuleForEach(x => x.RiskIndicators)
                .IsInEnum()
                .WithMessage("Invalid risk indicator.");
        }
    }
}
