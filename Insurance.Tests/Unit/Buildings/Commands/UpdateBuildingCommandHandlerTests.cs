using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Insurance.Application.Abstractions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Buildings.Commands;
using Insurance.Application.Buildings.DTOs;
using Insurance.Domain.Buildings;
using Insurance.Domain.Exceptions;
using AutoMapper;

namespace Insurance.Tests.Unit.Buildings.Commands
{
    public class UpdateBuildingCommandHandlerTests
    {
        private readonly Mock<IBuildingRepository> _buildingRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _uowMock = new();

        private readonly UpdateBuildingCommandHandler _handler;

        public UpdateBuildingCommandHandlerTests()
        {
            _handler = new UpdateBuildingCommandHandler(
                _buildingRepositoryMock.Object,
                _uowMock.Object);
        }

        private static UpdateBuildingCommand CreateValidCommand(Guid buildingId)
        {
            return new UpdateBuildingCommand(buildingId, new UpdateBuildingDto
            {
                InsuredValue = 200000,
                SurfaceArea = 150
            });
        }

        private static Building CreateExistingBuilding(Guid id)
        {
            return new Building
            {
                Id = id,
                SurfaceArea = 120,
                InsuredValue = 100000
            };
        }

        [Fact]
        public async Task Given_NonExistingBuilding_Should_ThrowNotFoundException()
        {

            var buildingId = Guid.NewGuid();

            _buildingRepositoryMock
                .Setup(r => r.GetBuildingByIdAsync(buildingId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Building)null!);

            var command = CreateValidCommand(buildingId);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));

            _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Given_ValidUpdate_Should_UpdateAndSave()
        {
            var buildingId = Guid.NewGuid();
            var existing = CreateExistingBuilding(buildingId);

            _buildingRepositoryMock
                .Setup(r => r.GetBuildingByIdAsync(buildingId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existing);

            var command = CreateValidCommand(buildingId);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(buildingId, result);

            _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
