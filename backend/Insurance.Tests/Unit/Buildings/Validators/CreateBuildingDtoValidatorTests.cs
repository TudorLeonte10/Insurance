using FluentValidation.TestHelper;
using Insurance.Application.Buildings.DTOs;
using Insurance.Application.Buildings.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.Buildings.Validators
{
    public class CreateBuildingDtoValidatorTests
    {
        private readonly CreateBuildingDtoValidator _validator;

        public CreateBuildingDtoValidatorTests()
        {
            _validator = new CreateBuildingDtoValidator();
        }

        [Fact]
        public void Given_NegativeInsuredValue_Should_FailValidation()
        {
            var dto = new CreateBuildingDto
            {
                InsuredValue = -1000,
                CityId = Guid.NewGuid(),
                SurfaceArea = 120
            };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.InsuredValue);
        }

        [Fact]
        public void Given_MissingCity_Should_FailValidation()
        {
            var dto = new CreateBuildingDto
            {
                InsuredValue = 100000,
                CityId = Guid.Empty
            };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.CityId);
        }


    }
}
