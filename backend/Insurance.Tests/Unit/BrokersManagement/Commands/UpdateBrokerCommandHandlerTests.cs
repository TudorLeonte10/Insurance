using Insurance.Application.Abstractions;
using Insurance.Application.Brokers.Commands;
using Insurance.Application.Brokers.DTOs;
using Insurance.Application.Exceptions;
using Insurance.Domain.Brokers;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.BrokersManagement.Commands
{
    public class UpdateBrokerCommandHandlerTests
    {
        private readonly Mock<IBrokerRepository> _brokerRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly UpdateBrokerCommandHandler _handler;

        public UpdateBrokerCommandHandlerTests()
        {
            _brokerRepositoryMock = new Mock<IBrokerRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _handler = new UpdateBrokerCommandHandler(
                _brokerRepositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Update_Broker_When_Found()
        {
            var broker = Broker.Rehydrate(
                Guid.NewGuid(),
                "BR001",
                "Old Name",
                "old@test.com",
                "123",
                true);

            _brokerRepositoryMock
                .Setup(x => x.GetByIdAsync(broker.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(broker);

            var dto = new UpdateBrokerDto
            {
                Name = "New Name",
                Email = "new@test.com",
                Phone = "456"
            };

            var command = new UpdateBrokerCommand(dto, broker.Id);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(broker.Id, result);

            _brokerRepositoryMock.Verify(
                x => x.UpdateAsync(broker, It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFound_When_Broker_Does_Not_Exist()
        {
            _brokerRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Broker)null!);

            var command = new UpdateBrokerCommand(
                new UpdateBrokerDto(), Guid.NewGuid());

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

    }
}
