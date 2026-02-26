using FluentValidation.TestHelper;
using Insurance.Application.Buildings.DTOs;
using Insurance.Application.Buildings.Validators;
using Insurance.Domain.Buildings;
using Xunit;

namespace Insurance.Tests.Unit.Buildings.Validators
{
    public class UpdateBuildingDtoValidatorTests
    {
        private readonly UpdateBuildingDtoValidator _validator;

        public UpdateBuildingDtoValidatorTests()
        {
            _validator = new UpdateBuildingDtoValidator();
        }

        [Fact]
        public void Given_NegativeInsuredValue_Should_FailValidation()
        {
            var dto = new UpdateBuildingDto
            {
                InsuredValue = -1,
                SurfaceArea = 120
            };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.InsuredValue);
        }

        [Fact]
        public void Given_ValidDto_Should_PassValidation()
        {
            var dto = new UpdateBuildingDto
            {
                Street = "Test Street",
                Number = "10A",
                ConstructionYear = 2000,
                SurfaceArea = 120,
                NumberOfFloors = 2,
                InsuredValue = 100000
            };

            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveAnyValidationErrors();
        }

    }
}
