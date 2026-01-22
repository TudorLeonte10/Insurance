using AutoMapper;
using Insurance.Application.Abstractions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Buildings.Commands;
using Insurance.Application.Buildings.DTOs;
using Insurance.Domain.Abstractions.Repositories;
using Insurance.Domain.Buildings;
using Insurance.Domain.Exceptions;
using Insurance.Domain.RiskIndicators;
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
        private readonly Mock<IGeographyRepository> _geographyRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        private readonly CreateBuildingCommandHandler _handler;

        public CreateBuildingCommandHandlerTests()
        {
            _handler = new CreateBuildingCommandHandler(
                _buildingRepositoryMock.Object,
                _clientRepositoryMock.Object,
                _geographyRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object);
        }

        private static CreateBuildingCommand CreateValidCommand(Guid clientId, Guid cityId)
        {
            return new CreateBuildingCommand(
                clientId,
                new CreateBuildingDto
                {
                    CityId = cityId,
                    InsuredValue = 100000,
                    SurfaceArea = 120,
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

            _clientRepositoryMock
                .Setup(x => x.ExistsAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var command = CreateValidCommand(clientId, Guid.NewGuid());

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Given_NonExistingCity_Should_ThrowNotFoundException()
        {
            var clientId = Guid.NewGuid();
            var cityId = Guid.NewGuid();

            _clientRepositoryMock
                .Setup(x => x.ExistsAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _geographyRepositoryMock
                .Setup(x => x.ExistsCityAsync(cityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var command = CreateValidCommand(clientId, cityId);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Given_ValidBuilding_Should_CreateBuildingSuccessfully()
        {
            var clientId = Guid.NewGuid();
            var cityId = Guid.NewGuid();
            var buildingId = Guid.NewGuid();

            var building = new Building { Id = buildingId };

            _clientRepositoryMock
                .Setup(x => x.ExistsAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _geographyRepositoryMock
                .Setup(x => x.ExistsCityAsync(cityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mapperMock
                .Setup(x => x.Map<Building>(It.IsAny<CreateBuildingDto>()))
                .Returns(building);

            var command = CreateValidCommand(clientId, cityId);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(buildingId, result);

            _buildingRepositoryMock.Verify(
                x => x.AddBuildingAsync(building, It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Given_DuplicateRiskIndicators_Should_SaveDistinctOnBuilding()
        {
            var clientId = Guid.NewGuid();
            var cityId = Guid.NewGuid();
            var buildingId = Guid.NewGuid();

            var building = new Building { Id = buildingId };

            _clientRepositoryMock
                .Setup(x => x.ExistsAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _geographyRepositoryMock
                .Setup(x => x.ExistsCityAsync(cityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mapperMock
                .Setup(x => x.Map<Building>(It.IsAny<CreateBuildingDto>()))
                .Returns(building);

            var command = new CreateBuildingCommand(
                clientId,
                new CreateBuildingDto
                {
                    CityId = cityId,
                    InsuredValue = 100000,
                    SurfaceArea = 120,
                    RiskIndicators = new[]
                    {
                RiskIndicatorType.FireRisk,
                RiskIndicatorType.FireRisk, // duplicate
                RiskIndicatorType.FloodRisk
                    }
                });

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(buildingId, result);
            Assert.Equal(2, building.RiskIndicators.Count);

            _unitOfWorkMock.Verify(
                u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }


    }
}
