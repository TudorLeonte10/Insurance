using FluentValidation.TestHelper;
using Insurance.Application.Buildings.Commands;
using Insurance.Application.Buildings.DTOs;
using Insurance.Application.Buildings.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.Buildings.Validators
{
    public class UpdateBuildingCommandValidatorTests
    {
        private readonly UpdateBuildingCommandValidator _validator;

        public UpdateBuildingCommandValidatorTests()
        {
            _validator = new UpdateBuildingCommandValidator();
        }

        [Fact]
        public void Should_Have_Error_When_BuildingId_Is_Empty()
        {
            var command = new UpdateBuildingCommand(
                Guid.Empty,
                new UpdateBuildingDto());

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.BuildingId)
                .WithErrorMessage("BuildingId is required.");
        }

        [Fact]
        public void Should_Have_Error_When_BuildingDto_Is_Null()
        {
            var command = new UpdateBuildingCommand(
                Guid.NewGuid(),
                null!);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.BuildingDto);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Command_Is_Valid()
        {
            var command = new UpdateBuildingCommand(
                Guid.NewGuid(),
                new UpdateBuildingDto
                {
                    Street = "Main St",
                    Number = "123",
                    ConstructionYear = 2000,
                    NumberOfFloors = 2,
                    SurfaceArea = 120,
                    InsuredValue = 150000
                });

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
