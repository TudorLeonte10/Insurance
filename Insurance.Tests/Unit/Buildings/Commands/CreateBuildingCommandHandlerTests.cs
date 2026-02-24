using AutoMapper;
using Insurance.Application.Abstractions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Authentication;
using Insurance.Application.Buildings.Commands;
using Insurance.Application.Buildings.DTOs;
using Insurance.Application.Exceptions;
using Insurance.Application.Geography.DTOs;
using Insurance.Domain.Buildings;
using Insurance.Domain.Clients;
using Insurance.Domain.Exceptions;
using Insurance.Domain.RiskIndicators;
using Insurance.Infrastructure.Persistence.Repositories;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.Unit.Buildings.Commands
{
    public class CreateBuildingCommandHandlerTests
    {
        private readonly Mock<IBuildingRepository> _buildingRepositoryMock = new();
        private readonly Mock<IClientRepository> _clientRepositoryMock = new();
        private readonly Mock<IGeographyReadRepository> _geographyRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<ICurrentUserContext> _currentUserContextMock = new();

        private readonly CreateBuildingCommandHandler _handler;

        public CreateBuildingCommandHandlerTests()
        {
            _handler = new CreateBuildingCommandHandler(
                _buildingRepositoryMock.Object,
                _clientRepositoryMock.Object,
                _geographyRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _currentUserContextMock.Object);
        }

        private static CreateBuildingCommand CreateValidCommand(Guid clientId, Guid cityId)
        {
            return new CreateBuildingCommand(
                clientId,
                new CreateBuildingDto
                {
                    CityId = cityId,
                    Type = BuildingType.Residential,
                    Street = "Main",
                    Number = "10",
                    ConstructionYear = 2000,
                    NumberOfFloors = 2,
                    SurfaceArea = 120,
                    InsuredValue = 100000,
                    RiskIndicators = new[]
                    {
                        RiskIndicatorType.FireRisk,
                        RiskIndicatorType.FloodRisk
                    }
                });
        }

        [Fact]
        public async Task Given_NonExistingClient_Should_ThrowNotFoundException()
        {
            var clientId = Guid.NewGuid();
            var brokerId = Guid.NewGuid();

            // ensure current user has a broker id so handler proceeds to client check
            _currentUserContextMock
                .SetupGet(x => x.BrokerId)
                .Returns(brokerId);

            _clientRepositoryMock
                .Setup(x => x.GetByIdAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Client?)null);

            var command = CreateValidCommand(clientId, Guid.NewGuid());

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }


        [Fact]
        public async Task Given_NonExistingCity_Should_ThrowNotFoundException()
        {
            var clientId = Guid.NewGuid();
            var cityId = Guid.NewGuid();
            var brokerId = Guid.NewGuid();

            _currentUserContextMock
                .SetupGet(x => x.BrokerId)
                .Returns(brokerId);

            var client = Client.Create(
                ClientType.Individual,
                brokerId,
                "John Doe",
                "123456789",
                "john@test.com",
                "0712345678",
                "Some address");

            _clientRepositoryMock
                .Setup(x => x.GetByIdAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);


            _geographyRepositoryMock
                .Setup(x => x.GetCityByIdAsync(cityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CityDto?)null);

            var command = CreateValidCommand(clientId, cityId);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }


        [Fact]
        public async Task Given_ValidBuilding_Should_CreateBuildingSuccessfully()
        {
            var clientId = Guid.NewGuid();
            var cityId = Guid.NewGuid();
            var brokerId = Guid.NewGuid();

            _currentUserContextMock
                .SetupGet(x => x.BrokerId)
                .Returns(brokerId);

            var client = Client.Create(
                ClientType.Individual,
                brokerId,
                "John Doe",
                "123456789",
                "john@test.com",
                "0712345678",
                "Some address");

            _clientRepositoryMock
                .Setup(x => x.GetByIdAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);

            _geographyRepositoryMock
                .Setup(x => x.GetCityByIdAsync(cityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CityDto(cityId, "test city"));

            _buildingRepositoryMock
                .Setup(x => x.AddAsync(
                    It.IsAny<Building>(),
                    It.IsAny<IReadOnlyCollection<RiskIndicatorType>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = CreateValidCommand(clientId, cityId);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotEqual(Guid.Empty, result);

            _buildingRepositoryMock.Verify(
                x => x.AddAsync(
                    It.IsAny<Building>(),
                    It.Is<IReadOnlyCollection<RiskIndicatorType>>(r => r.Count == 2),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Given_DuplicateRiskIndicators_Should_PassDistinctToRepository()
        {
            var clientId = Guid.NewGuid();
            var cityId = Guid.NewGuid();
            var brokerId = Guid.NewGuid();

            _currentUserContextMock
                .SetupGet(x => x.BrokerId)
                .Returns(brokerId);

            var client = Client.Create(
                ClientType.Individual,
                brokerId,
                "John Doe",
                "123456789",
                "john@test.com",
                "0712345678",
                "Some address");

            _clientRepositoryMock
                .Setup(x => x.GetByIdAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);

            _geographyRepositoryMock
                .Setup(x => x.GetCityByIdAsync(cityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CityDto(cityId, "test city"));

            IReadOnlyCollection<RiskIndicatorType>? passedIndicators = null;

            _buildingRepositoryMock
                .Setup(x => x.AddAsync(
                    It.IsAny<Building>(),
                    It.IsAny<IReadOnlyCollection<RiskIndicatorType>>(),
                    It.IsAny<CancellationToken>()))
                .Callback<Building, IReadOnlyCollection<RiskIndicatorType>, CancellationToken>(
                    (_, indicators, _) => passedIndicators = indicators)
                .Returns(Task.CompletedTask);

            var command = new CreateBuildingCommand(
                clientId,
                new CreateBuildingDto
                {
                    CityId = cityId,
                    Type = BuildingType.Residential,
                    Street = "Main",
                    Number = "10",
                    ConstructionYear = 2000,
                    NumberOfFloors = 2,
                    SurfaceArea = 120,
                    InsuredValue = 100000,
                    RiskIndicators = new[]
                    {
                        RiskIndicatorType.FireRisk,
                        RiskIndicatorType.FireRisk,
                        RiskIndicatorType.FloodRisk
                    }
                });

            await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(passedIndicators);
            Assert.Equal(2, passedIndicators!.Count);
        }



    }
}
