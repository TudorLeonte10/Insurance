using FluentValidation.TestHelper;
using Insurance.Application.Clients.DTOs;
using Insurance.Application.Clients.Validators;
using Xunit;

namespace Insurance.Tests.Unit.Clients.Validators
{
    public class UpdateClientDtoValidatorTests
    {
        private readonly UpdateClientDtoValidator _validator;

        public UpdateClientDtoValidatorTests()
        {
            _validator = new UpdateClientDtoValidator();
        }

        [Fact]
        public void Given_EmptyName_Should_FailValidation()
        {
            var dto = new UpdateClientDto
            {
                Name = "",
                Email = "test@test.ro",
                PhoneNumber = "0712345678"
            };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Given_InvalidEmail_Should_FailValidation()
        {
            var dto = new UpdateClientDto
            {
                Name = "Valid Name",
                Email = "not-an-email",
                PhoneNumber = "0712345678"
            };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Given_EmptyPhoneNumber_Should_FailValidation()
        {
            var dto = new UpdateClientDto
            {
                Name = "Valid Name",
                Email = "test@test.ro",
                PhoneNumber = ""
            };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
        }

        [Fact]
        public void Given_ValidDto_Should_PassValidation()
        {
            var dto = new UpdateClientDto
            {
                Name = "Valid Name",
                Email = "valid@test.ro",
                PhoneNumber = "0712345678"
            };

            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
