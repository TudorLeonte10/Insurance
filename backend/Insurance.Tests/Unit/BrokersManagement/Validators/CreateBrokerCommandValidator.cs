using Insurance.Application.Brokers.Commands;
using Insurance.Application.Brokers.DTOs;
using Insurance.Application.Brokers.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.BrokersManagement.Validators
{
    public class CreateBrokerCommandValidatorTests
    {
        private readonly CreateBrokerCommandValidator _validator =
            new CreateBrokerCommandValidator();

        [Fact]
        public void Should_fail_when_email_is_empty()
        {
            var dto = new CreateBrokerDto
            {
                BrokerCode = "BR001",
                Name = "Test",
                Email = "",
                Phone = "123"
            };

            var command = new CreateBrokerCommand(dto);

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(
                result.Errors,
                e => e.PropertyName.Contains("Email"));
        }

        [Fact]
        public void Should_pass_when_command_is_valid()
        {
            var dto = new CreateBrokerDto
            {
                BrokerCode = "BR001",
                Name = "Test",
                Email = "test@test.com",
                Phone = "123"
            };

            var command = new CreateBrokerCommand(dto);

            var result = _validator.Validate(command);

            Assert.True(result.IsValid);
        }
    }


}
