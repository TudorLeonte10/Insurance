using AutoMapper;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Buildings.DTOs;
using Insurance.Application.Buildings.Queries;
using Insurance.Domain.Abstractions.Repositories;
using Insurance.Domain.Buildings;
using Insurance.Domain.Exceptions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.Unit.Buildings.Queries
{
    public class GetBuildingByIdQueryHandlerTests
    {
        private readonly Mock<IBuildingRepository> _buildingRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        private readonly GetBuildingByIdQueryHandler _handler;

        public GetBuildingByIdQueryHandlerTests()
        {
            _handler = new GetBuildingByIdQueryHandler(
                _buildingRepositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Given_ExistingBuilding_Should_ReturnBuildingDetailsDto()
        {
            var buildingId = Guid.NewGuid();

            var building = new Building
            {
                Id = buildingId
            };

            var dto = new BuildingDetailsDto
            {
                Id = buildingId
            };

            _buildingRepositoryMock
                .Setup(r => r.GetBuildingByIdAsync(buildingId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(building);

            _mapperMock
                .Setup(m => m.Map<BuildingDetailsDto>(building))
                .Returns(dto);

            var query = new GetBuildingByIdQuery(buildingId);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(buildingId, result.Id);
        }

        [Fact]
        public async Task Given_NonExistingBuilding_Should_ThrowNotFoundException()
        {
            var buildingId = Guid.NewGuid();

            _buildingRepositoryMock
                .Setup(r => r.GetBuildingByIdAsync(buildingId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Building)null!);

            var query = new GetBuildingByIdQuery(buildingId);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
    }
}
