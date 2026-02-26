using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Brokers.DTOs;
using Insurance.Application.Brokers.Queries;
using Insurance.Application.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.BrokersManagement.Queries
{
    public class GetBrokerByIdQueryHandlerTests
    {
        private readonly Mock<IBrokerReadRepository> _readRepositoryMock;
        private readonly GetBrokerByIdQueryHandler _handler;

        public GetBrokerByIdQueryHandlerTests()
        {
            _readRepositoryMock = new Mock<IBrokerReadRepository>();
            _handler = new GetBrokerByIdQueryHandler(_readRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_Broker_When_Found()
        {
            var brokerId = Guid.NewGuid();

            var broker = new BrokerDetailsDto
            {
                Id = brokerId,
                BrokerCode = "BR001",
                Name = "Broker",
                Email = "test@test.com",
                Phone = "123",
                IsActive = true
            };

            _readRepositoryMock
                .Setup(x => x.GetByIdAsync(brokerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(broker);

            var query = new GetBrokerByIdQuery(brokerId);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(brokerId, result.Id);
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFound_When_Broker_Does_Not_Exist()
        {
            _readRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((BrokerDetailsDto)null!);

            var query = new GetBrokerByIdQuery(Guid.NewGuid());

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
    }

}
