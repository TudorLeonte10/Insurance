using Insurance.Application.Abstractions;
using Insurance.Application.Brokers.Commands;
using Insurance.Application.Brokers.DTOs;
using Insurance.Domain.Brokers;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.BrokersManagement.Commands
{
    public class CreateBrokerCommandHandlerTests
    {
        private readonly Mock<IBrokerRepository> _repoMock = new();
        private readonly Mock<IUnitOfWork> _uowMock = new();

        private CreateBrokerCommandHandler _handler;
        public CreateBrokerCommandHandlerTests()
        {
            _handler = new CreateBrokerCommandHandler(_repoMock.Object, _uowMock.Object);
        }
        [Fact]
        public async Task Should_create_broker_when_data_is_valid()
        {

            var dto = new CreateBrokerDto
            {
                BrokerCode = "BR001",
                Name = "Test",
                Email = "test@test.com",
                Phone = "123"
            };

            var command = new CreateBrokerCommand(dto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotEqual(Guid.Empty, result);
        }

        [Fact]
        public async Task Should_call_repository_add_and_unit_of_work_save()
        {
            var dto = new CreateBrokerDto
            {
                BrokerCode = "BR002",
                Name = "Test Broker",
                Email = "broker@test.com",
                Phone = "0712345678"
            };

            _repoMock
                .Setup(x => x.AddAsync(It.IsAny<Broker>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new CreateBrokerCommand(dto);

            await _handler.Handle(command, CancellationToken.None);

            _repoMock.Verify(
                x => x.AddAsync(It.IsAny<Broker>(), It.IsAny<CancellationToken>()),
                Times.Once);

            _uowMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Should_pass_correct_broker_data_to_repository()
        {
            var dto = new CreateBrokerDto
            {
                BrokerCode = "BR003",
                Name = "Correct Broker",
                Email = "correct@test.com",
                Phone = "0723456789"
            };

            Broker capturedBroker = null!;

            _repoMock
                .Setup(x => x.AddAsync(It.IsAny<Broker>(), It.IsAny<CancellationToken>()))
                .Callback<Broker, CancellationToken>((broker, ct) => capturedBroker = broker)
                .Returns(Task.CompletedTask);

            var command = new CreateBrokerCommand(dto);

            await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(capturedBroker);
            Assert.Equal(dto.BrokerCode, capturedBroker.BrokerCode);
            Assert.Equal(dto.Name, capturedBroker.Name);
            Assert.Equal(dto.Email, capturedBroker.Email);
            Assert.Equal(dto.Phone, capturedBroker.Phone);
        }

    }

}
