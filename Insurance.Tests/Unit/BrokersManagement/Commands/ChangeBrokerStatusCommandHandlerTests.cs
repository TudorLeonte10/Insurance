using Insurance.Application.Abstractions;
using Insurance.Application.Brokers.Commands;
using Insurance.Application.Exceptions;
using Insurance.Domain.Brokers;
using Insurance.Domain.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.BrokersManagement.Commands
{
    public class ChangeBrokerStatusCommandHandlerTests
    {
        private readonly Mock<IBrokerRepository> _brokerRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly ChangeBrokerStatusCommandHandler _handler;

        public ChangeBrokerStatusCommandHandlerTests()
        {
            _brokerRepositoryMock = new Mock<IBrokerRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _handler = new ChangeBrokerStatusCommandHandler(
                _brokerRepositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Activate_Broker_When_Inactive()
        {
            var broker = Broker.Rehydrate(
                Guid.NewGuid(),
                "BR001",
                "Test Broker",
                "test@test.com",
                "123",
                isActive: false);

            _brokerRepositoryMock
                .Setup(x => x.GetByIdAsync(broker.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(broker);

            var command = new ChangeBrokerStatusCommand(broker.Id, true);

            await _handler.Handle(command, CancellationToken.None);

            Assert.True(broker.IsActive);

            _brokerRepositoryMock.Verify(
                x => x.UpdateAsync(broker, It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Deactivate_Broker_When_Active()
        {
            var broker = Broker.Rehydrate(
                Guid.NewGuid(),
                "BR001",
                "Test Broker",
                "test@test.com",
                "123",
                isActive: true);

            _brokerRepositoryMock
                .Setup(x => x.GetByIdAsync(broker.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(broker);

            var command = new ChangeBrokerStatusCommand(broker.Id, false);

            await _handler.Handle(command, CancellationToken.None);

            Assert.False(broker.IsActive);
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFound_When_Broker_Does_Not_Exist()
        {
            _brokerRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Broker)null);

            var command = new ChangeBrokerStatusCommand(Guid.NewGuid(), true);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Activating_Already_Active_Broker()
        {
            var broker = Broker.Rehydrate(
                Guid.NewGuid(),
                "BR001",
                "Test Broker",
                "test@test.com",
                "123",
                isActive: true);

            _brokerRepositoryMock
                .Setup(x => x.GetByIdAsync(broker.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(broker);

            var command = new ChangeBrokerStatusCommand(broker.Id, true);

            await Assert.ThrowsAsync<BrokerAlreadyActiveException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
    }
