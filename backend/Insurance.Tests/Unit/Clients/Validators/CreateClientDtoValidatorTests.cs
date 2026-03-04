using FluentValidation.TestHelper;
using Insurance.Application.Clients.DTOs;
using Insurance.Application.Clients.Validators;
using Insurance.Domain.Clients;

namespace Insurance.Tests.Unit.Clients.Validators
{
    public class CreateClientDtoValidatorTests
    {
        private readonly CreateClientDtoValidator _validator;

        public CreateClientDtoValidatorTests()
        {
            _validator = new CreateClientDtoValidator();
        }

        [Fact]
        public void Given_EmptyName_Should_Have_Error()
        {
            var dto = new CreateClientDto
            {
                Name = "",
                IdentificationNumber = "123456789",
                Email = "test@test.ro",
                PhoneNumber = "0712345678",
                Type = ClientType.Individual
            };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Given_MissingIdentificationNumber_Should_FailValidation()
        {
            var dto = new CreateClientDto
            {
                Name = "Test Client",
                IdentificationNumber = "",
                Email = "test@test.ro",
                PhoneNumber = "0712345678",
                Type = ClientType.Individual
            };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.IdentificationNumber);
        }

        [Fact]
        public void Given_InvalidEmail_Should_FailValidation()
        {
            var dto = new CreateClientDto
            {
                Name = "Test Client",
                IdentificationNumber = "123456789",
                Email = "not-an-email",
                PhoneNumber = "0712345678",
                Type = ClientType.Individual
            };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }
    }
}
