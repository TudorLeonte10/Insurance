using Insurance.Application.Brokers.Commands;
using Insurance.Application.Brokers.DTOs;
using Insurance.Application.Brokers.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.BrokersManagement.Validators
{
    public class UpdateBrokerCommandValidatorTests
    {
        private readonly UpdateBrokerCommandValidator _validator =
            new UpdateBrokerCommandValidator();

        [Fact]
        public void Should_fail_when_email_is_empty()
        {
            var dto = new UpdateBrokerDto
            {
                Name = "Test",
                Email = "",
                Phone = "123"
            };
            var command = new UpdateBrokerCommand(dto, Guid.NewGuid());
            var result = _validator.Validate(command);
            Assert.False(result.IsValid);
            Assert.Contains(
                result.Errors,
                e => e.PropertyName.Contains("Email"));
        }

        [Fact]
        public void Should_pass_when_command_is_valid()
        {
            var dto = new UpdateBrokerDto
            {
                Name = "Test",
                Email = "test@example.com",
                Phone = "123"
            };
            var command = new UpdateBrokerCommand(dto, Guid.NewGuid());
            var result = _validator.Validate(command);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Should_fail_when_name_is_empty()
        {
            var dto = new UpdateBrokerDto
            {
                Name = "",
                Email = "ieas@test.com",
                Phone = "123"
            };
            var command = new UpdateBrokerCommand(dto, Guid.NewGuid());
            var result = _validator.Validate(command);
            Assert.False(result.IsValid);
        }
    }
}
