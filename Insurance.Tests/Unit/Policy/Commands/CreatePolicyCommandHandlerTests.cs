using Insurance.Application.Abstractions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Brokers;
using Insurance.Application.Buildings;
using Insurance.Application.Buildings.DTOs;
using Insurance.Application.Clients;
using Insurance.Application.Common;
using Insurance.Application.Exceptions;
using Insurance.Application.Policy.Commands;
using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Services;
using Insurance.Domain.Brokers;
using Insurance.Domain.Clients;
using Insurance.Domain.Metadata;
using Insurance.Domain.Policies;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.Unit.Policy.Commands
{
    public class CreatePolicyCommandHandlerTests
    {
        private readonly Mock<IClientRepository> _clientRepo = new();
        private readonly Mock<IBrokerRepository> _brokerRepo = new();
        private readonly Mock<ICurrencyRepository> _currencyRepo = new();
        private readonly Mock<IBuildingReadRepository> _buildingReadRepo = new();
        private readonly Mock<IFeeConfigurationReadRepository> _feeRepo = new();
        private readonly Mock<IRiskFactorReadRepository> _riskRepo = new();
        private readonly Mock<IPolicyPremiumCalculator> _calculator = new();
        private readonly Mock<IPolicyRepository> _policyRepo = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<TimeProvider> _timeProvider = new();

        private readonly CreatePolicyCommandHandler _handler;

        public CreatePolicyCommandHandlerTests()
        {
            _timeProvider.Setup(x => x.GetUtcNow()).Returns(DateTime.UtcNow);

            _handler = new CreatePolicyCommandHandler(
                _clientRepo.Object,
                _buildingReadRepo.Object,
                _brokerRepo.Object,
                _currencyRepo.Object,
                _feeRepo.Object,
                _riskRepo.Object,
                _calculator.Object,
                _policyRepo.Object,
                _unitOfWork.Object,
                _timeProvider.Object
            );
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFound_When_Client_Does_Not_Exist()
        {
            var command = CreateValidCommand();

            _clientRepo
                .Setup(x => x.GetByIdAsync(
                    command.PolicyDto.ClientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Insurance.Domain.Clients.Client?)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFound_When_Broker_Does_Not_Exist()
        {
            var command = CreateValidCommand();

            _clientRepo
                .Setup(x => x.GetByIdAsync(
                    command.PolicyDto.ClientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Insurance.Domain.Clients.Client?)null);

            _brokerRepo
                .Setup(x => x.GetByIdAsync(
                    command.PolicyDto.BrokerId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Insurance.Domain.Brokers.Broker?)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFound_When_Currency_Does_Not_Exist()
        {
            var command = CreateValidCommand();

            _clientRepo
                .Setup(x => x.GetByIdAsync(
                    command.PolicyDto.ClientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Client)null!);

            _brokerRepo
                .Setup(x => x.GetByIdAsync(
                    command.PolicyDto.BrokerId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Broker)null!);

            _currencyRepo
                .Setup(x => x.GetByIdAsync(
                    command.PolicyDto.CurrencyId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Insurance.Domain.Metadata.Currency?)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFound_When_Building_Not_Found()
        {
            var command = CreateValidCommand();

            _clientRepo
                .Setup(x => x.GetByIdAsync(
                    command.PolicyDto.ClientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Client)null!);

            _brokerRepo
                .Setup(x => x.GetByIdAsync(
                    command.PolicyDto.BrokerId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Broker)null!);

            _currencyRepo
                .Setup(x => x.GetByIdAsync(
                    command.PolicyDto.CurrencyId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Metadata.Currency)null!);

            _buildingReadRepo
                .Setup(x => x.GetGeoContextAsync(
                    command.PolicyDto.BuildingId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((BuildingGeoContextDto?)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        private static CreatePolicyCommand CreateValidCommand()
        {
            var dto = new CreatePolicyDto
            {
                ClientId = Guid.NewGuid(),
                BuildingId = Guid.NewGuid(),
                BrokerId = Guid.NewGuid(),
                CurrencyId = Guid.NewGuid(),
                BasePremium = 10000,
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(365)
            };

            return new CreatePolicyCommand(dto);
        }
    }
}
