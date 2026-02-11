using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Buildings.DTOs;
using Insurance.Application.Buildings.Queries;
using Insurance.Application.Exceptions;
using Insurance.Domain.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.Unit.Buildings.Queries
{
    public class GetBuildingsQueryHandlerTests
    {
        private readonly Mock<IBuildingReadRepository> _repositoryMock = new();
        private readonly GetBuildingsQueryHandler _handler;

        public GetBuildingsQueryHandlerTests()
        {
            _handler = new GetBuildingsQueryHandler(
                _repositoryMock.Object);
        }

        [Fact]
        public async Task Given_BuildingId_When_BuildingExists_Should_ReturnSingleItem()
        {
            var buildingId = Guid.NewGuid();

            var dto = new BuildingDetailsDto
            {
                Id = buildingId,
                Type = "Residential"
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(buildingId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(dto);

            var query = new GetBuildingsQuery
            {
                BuildingId = buildingId
            };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal(buildingId, result[0].Id);

            _repositoryMock.Verify(
                r => r.GetByIdAsync(buildingId, It.IsAny<CancellationToken>()),
                Times.Once);

            _repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Given_BuildingId_When_BuildingDoesNotExist_Should_ThrowNotFoundException()
        {
            var buildingId = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.GetByIdAsync(buildingId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((BuildingDetailsDto?)null);

            var query = new GetBuildingsQuery
            {
                BuildingId = buildingId
            };

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Given_ClientId_Should_ReturnBuildingsForClient()
        {
            var clientId = Guid.NewGuid();

            var buildings = new List<BuildingDetailsDto>
            {
                new BuildingDetailsDto { Id = Guid.NewGuid(), Type = "Residential" },
                new BuildingDetailsDto { Id = Guid.NewGuid(), Type = "Office" }
            };

            _repositoryMock
                .Setup(r => r.GetByClientIdAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(buildings);

            var query = new GetBuildingsQuery
            {
                ClientId = clientId
            };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(2, result.Count);

            _repositoryMock.Verify(
                r => r.GetByClientIdAsync(clientId, It.IsAny<CancellationToken>()),
                Times.Once);

            _repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Given_NoIdentifiers_Should_ThrowArgumentException()
        {
            var query = new GetBuildingsQuery();

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
    }
}
