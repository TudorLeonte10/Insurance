using FluentValidation.TestHelper;
using Insurance.Application.Clients.Commands;
using Insurance.Application.Clients.DTOs;
using Insurance.Application.Clients.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.Clients.Validators
{
    public class UpdateClientCommandValidatorTests
    {
        private readonly UpdateClientCommandValidator _validator;

        public UpdateClientCommandValidatorTests()
        {
            _validator = new UpdateClientCommandValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Dto_Is_Null()
        {
            var command = new UpdateClientCommand(
                Guid.NewGuid(),
                null!);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Dto);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Command_Is_Valid()
        {
            var command = new UpdateClientCommand(
                Guid.NewGuid(),
                new UpdateClientDto
                {
                    Name = "Updated Name",
                    Email = "updated@test.ro",
                    PhoneNumber = "0712345678",
                    Address = "Updated Address",
                    IdentificationNumber = "1234567890123"
                });

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
