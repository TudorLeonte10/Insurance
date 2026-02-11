using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Xunit;
using Insurance.Application.Geography.Queries;
using Insurance.Application.Geography.DTOs;
using Insurance.Application.Abstractions.Repositories;

namespace Insurance.Tests.Unit.Geography.Queries
{
    public class GetCitiesByCountyQueryHandlerTests
    {
        private readonly Mock<IGeographyReadRepository> _repoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        private readonly GetCitiesByCountyQueryHandler _handler;

        public GetCitiesByCountyQueryHandlerTests()
        {
            _handler = new GetCitiesByCountyQueryHandler(
                _repoMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Given_CountyId_Should_ReturnCities()
        {
            var countyId = Guid.NewGuid();

            var cities = new List<CityDto>
            {
                new CityDto(Guid.NewGuid(), "City 1"),
                new CityDto(Guid.NewGuid(), "City 2")
            };

            _repoMock
                .Setup(r => r.GetCitiesByCountyIdAsync(countyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cities);

            _mapperMock
                .Setup(m => m.Map<IReadOnlyList<CityDto>>(cities))
                .Returns(cities);

            var query = new GetCitiesByCountyQuery(countyId);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(2, result.Count);
        }
    }
}

