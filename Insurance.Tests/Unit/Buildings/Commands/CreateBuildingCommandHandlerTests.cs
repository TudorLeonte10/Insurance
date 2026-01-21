using AutoMapper;
using FluentAssertions;
using Insurance.Application.Abstractions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Buildings.Commands;
using Insurance.Application.Buildings.DTOs;
using Insurance.Application.Clients.Commands;
using Insurance.Application.Clients.Commands.CreateClient;
using Insurance.Application.Clients.DTOs;
using Insurance.Domain.Abstractions.Repositories;
using Insurance.Domain.Buildings;
using Insurance.Domain.Clients;
using Insurance.Domain.Exceptions;
using Insurance.Domain.RiskIndicators;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Insurance.Tests.Unit.Buildings.Commands
{
    public class CreateBuildingCommandHandlerTests
    {
        private readonly Mock<IBuildingRepository> _buildingRepositoryMock;
        private readonly Mock<IClientRepository> _clientRepositoryMock;
        private readonly Mock<IGeographyRepository> _geographyRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly CreateBuildingCommandHandler _handler;

        public CreateBuildingCommandHandlerTests()
        {
            _buildingRepositoryMock = new Mock<IBuildingRepository>();
            _clientRepositoryMock = new Mock<IClientRepository>();
            _geographyRepositoryMock = new Mock<IGeographyRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _handler = new CreateBuildingCommandHandler(
                _buildingRepositoryMock.Object,
                _clientRepositoryMock.Object,
                _geographyRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object
            );
        }

        private CreateBuildingCommand CreateValidCommand(Guid? clientId = null, Guid? cityId = null)
        {
            return new CreateBuildingCommand(clientId ?? Guid.NewGuid(),
                   new CreateBuildingDto
                   {
                        CityId = cityId ?? Guid.NewGuid(),
                        InsuredValue = 100000,
                        SurfaceArea = 120,
                        RiskIndicators = new[] { RiskIndicatorType.FireRisk, RiskIndicatorType.FloodRisk }
                    });
        }


        [Fact]
        public async Task Given_NonExistingClient_Should_ThrowNotFoundException()
        {
            var clientId = Guid.NewGuid();

            _clientRepositoryMock
                .Setup(x => x.ExistsAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var command = CreateValidCommand(clientId);

            Func<Task> act = async () =>
                await _handler.Handle(command, CancellationToken.None);

            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage($"*{clientId}*");
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

            Func<Task> act = async () =>
                await _handler.Handle(command, CancellationToken.None);

            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage($"*{cityId}*");
        }

        [Fact]
        public async Task Given_ValidBuilding_Should_CreateBuildingSuccessfully()
        {
            var clientId = Guid.NewGuid();
            var cityId = Guid.NewGuid();
            var buildingId = Guid.NewGuid();

            var building = new Building
            {
                Id = buildingId
            };

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

            result.Should().Be(buildingId);

            _buildingRepositoryMock.Verify(
                x => x.AddBuildingAsync(building, It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
