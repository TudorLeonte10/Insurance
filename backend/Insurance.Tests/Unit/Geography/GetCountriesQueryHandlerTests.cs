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
    public class GetCountriesQueryHandlerTests
    {
        private readonly Mock<IGeographyReadRepository> _repoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        private readonly GetCountriesQueryHandler _handler;

        public GetCountriesQueryHandlerTests()
        {
            _handler = new GetCountriesQueryHandler(
                _repoMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Should_ReturnCountries()
        {
            var countries = new List<CountryDto>
            {
                new CountryDto(Guid.NewGuid(), "Romania"),
                new CountryDto(Guid.NewGuid(), "Germany")
            };

            _repoMock
                .Setup(r => r.GetCountriesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(countries);

            _mapperMock
                .Setup(m => m.Map<IReadOnlyList<CountryDto>>(countries))
                .Returns(countries);

            var query = new GetCountriesQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(2, result.Count);
        }
    }
}
