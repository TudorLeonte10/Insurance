using AutoMapper;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Buildings.DTOs;
using Insurance.Application.Buildings.Queries;
using Insurance.Domain.Abstractions.Repositories;
using Insurance.Domain.Buildings;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.Unit.Buildings.Queries
{
    public class GetBuildingsByClientQueryHandlerTests
    {
        private readonly Mock<IBuildingRepository> _buildingRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        private readonly GetBuildingsByClientQueryHandler _handler;

        public GetBuildingsByClientQueryHandlerTests()
        {
            _handler = new GetBuildingsByClientQueryHandler(
                _buildingRepositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Given_ClientWithBuildings_Should_ReturnBuildingDetailsList()
        {
            var clientId = Guid.NewGuid();

            var buildings = new List<Building>
            {
                new Building { Id = Guid.NewGuid() },
                new Building { Id = Guid.NewGuid() }
            };

            var dtos = new List<BuildingDetailsDto>
            {
                new BuildingDetailsDto { Id = buildings[0].Id },
                new BuildingDetailsDto { Id = buildings[1].Id }
            };

            _buildingRepositoryMock
                .Setup(r => r.GetAllBuildingsByClientIdAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(buildings);

            _mapperMock
                .Setup(m => m.Map<IReadOnlyList<BuildingDetailsDto>>(buildings))
                .Returns(dtos);

            var query = new GetBuildingsByClientQuery(clientId);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }
    }
}
