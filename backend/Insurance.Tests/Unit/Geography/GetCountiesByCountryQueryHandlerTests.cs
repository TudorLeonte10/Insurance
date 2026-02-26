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
    public class GetCountiesByCountryQueryHandlerTests
    {
        private readonly Mock<IGeographyReadRepository> _repoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        private readonly GetCountiesByCountryQueryHandler _handler;

        public GetCountiesByCountryQueryHandlerTests()
        {
            _handler = new GetCountiesByCountryQueryHandler(
                _repoMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Given_CountryId_Should_ReturnCounties()
        {
            var countryId = Guid.NewGuid();

            var counties = new List<CountyDto>
            {
                new CountyDto(Guid.NewGuid(), "County 1"),
                new CountyDto(Guid.NewGuid(), "County 2")
            };

            _repoMock
                .Setup(r => r.GetCountiesByCountryIdAsync(countryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(counties);

            _mapperMock
                .Setup(m => m.Map<IReadOnlyList<CountyDto>>(counties))
                .Returns(counties);

            var query = new GetCountiesByCountryQuery(countryId);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(2, result.Count);
        }
    }
}
