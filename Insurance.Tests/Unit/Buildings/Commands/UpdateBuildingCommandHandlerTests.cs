using Insurance.Application.Abstractions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Authentication;
using Insurance.Application.Buildings.Commands;
using Insurance.Application.Buildings.DTOs;
using Insurance.Application.Exceptions;
using Insurance.Domain.Buildings;
using Insurance.Domain.Clients;
using Insurance.Domain.Exceptions;
using Insurance.Domain.RiskIndicators;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.Unit.Buildings.Commands
{
    public class UpdateBuildingCommandHandlerTests
    {
        private readonly Mock<IBuildingRepository> _buildingRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _uowMock = new();
        private readonly Mock<ICurrentUserContext> _currentUserContextMock = new();
        private readonly Mock<IClientRepository> _clientRepositoryMock = new();

        private readonly UpdateBuildingCommandHandler _handler;

        public UpdateBuildingCommandHandlerTests()
        {
          
            _currentUserContextMock
                .SetupGet(x => x.BrokerId)
                .Returns(Guid.NewGuid());

            _handler = new UpdateBuildingCommandHandler(
                _buildingRepositoryMock.Object,
                _uowMock.Object,
                _currentUserContextMock.Object,
                _clientRepositoryMock.Object);
        }

        private static UpdateBuildingCommand CreateValidCommand(Guid buildingId)
        {
            return new UpdateBuildingCommand(
                buildingId,
                new UpdateBuildingDto
                {
                    Street = "Updated Street",
                    Number = "20A",
                    ConstructionYear = 2010,
                    NumberOfFloors = 3,
                    SurfaceArea = 150,
                    InsuredValue = 200000,
                });
        }

        private static Building CreateExistingBuilding(Guid id, Guid clientId)
        {
            return Building.Create(
                clientId: clientId,
                cityId: Guid.NewGuid(),
                type: BuildingType.Residential,
                street: "Old Street",
                number: "10",
                constructionYear: 2000,
                numberOfFloors: 2,
                surfaceArea: 120,
                insuredValue: 100000
            );
        }

        [Fact]
        public async Task Given_NonExistingBuilding_Should_ThrowNotFoundException()
        {
            var buildingId = Guid.NewGuid();

            _buildingRepositoryMock
                .Setup(r => r.GetByIdAsync(buildingId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Building?)null);

            var command = CreateValidCommand(buildingId);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));

            _uowMock.Verify(
                u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Given_ValidUpdate_Should_UpdateAndSave()
        {
            var buildingId = Guid.NewGuid();
            var brokerId = Guid.NewGuid();
            var clientId = Guid.NewGuid();

            _currentUserContextMock
                .SetupGet(x => x.BrokerId)
                .Returns(brokerId);

            var existing = CreateExistingBuilding(buildingId, clientId);

            _buildingRepositoryMock
                .Setup(r => r.GetByIdAsync(buildingId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existing);

            var client = Client.Rehydrate(
                clientId,
                brokerId,
                ClientType.Individual,
                "John Doe",
                "123456789",
                "john@test.com",
                "0712345678",
                "Some address");

            _clientRepositoryMock
                .Setup(c => c.GetByIdAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);

            _buildingRepositoryMock
                .Setup(r => r.UpdateAsync(
                    existing,
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = CreateValidCommand(buildingId);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(existing.Id, result);

            _buildingRepositoryMock.Verify(
                r => r.UpdateAsync(
                    existing,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _uowMock.Verify(
                u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
