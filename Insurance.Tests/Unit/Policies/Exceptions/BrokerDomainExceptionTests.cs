using Insurance.Domain.Brokers;
using Insurance.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.Policies.Exceptions
{
    public class BrokerDomainExceptionTests
    {
        [Fact]
        public void Deactivate_WhenAlreadyInactive_ShouldThrowBrokerAlreadyInactiveException()
        {
            var broker = Broker.Create(
                "BR001",
                "Test Broker",
                "broker@test.com",
                "0712345678");

            broker.Deactivate(); 

            var exception = Assert.Throws<BrokerAlreadyInactiveException>(() =>
                broker.Deactivate()); 

            Assert.NotNull(exception.Message);
        }

        [Fact]
        public void Activate_WhenAlreadyActive_ShouldThrowBrokerAlreadyActiveException()
        {
            var broker = Broker.Create(
                "BR001",
                "Test Broker",
                "broker@test.com",
                "0712345678");

            var exception = Assert.Throws<BrokerAlreadyActiveException>(() =>
                broker.Activate()); 

            Assert.NotNull(exception.Message);
        }
    }
}
