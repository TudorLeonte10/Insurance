using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Common.Paging;
using Insurance.Application.Metadata.Currency.DTOs;
using Insurance.Application.Metadata.Currency.Queries;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.Currency.Queries
{
    public class GetCurrenciesQueryHandlerTests
    {
        private readonly Mock<ICurrencyReadRepository> _readRepositoryMock;
        private readonly GetCurrenciesQueryHandler _handler;

        public GetCurrenciesQueryHandlerTests()
        {
            _readRepositoryMock = new Mock<ICurrencyReadRepository>();
            _handler = new GetCurrenciesQueryHandler(_readRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_PagedResult()
        {
            var expected = new PagedResult<CurrencyDto>(new List<CurrencyDto>(), 1, 10, 0);

            _readRepositoryMock
                .Setup(x => x.GetPagedAsync(1, 10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            var query = new GetCurrenciesQuery(1, 10);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task Handle_Should_Handle_Empty_Result()
        {
            var expected = new PagedResult<CurrencyDto>(new List<CurrencyDto>(), 1, 10, 0);
            _readRepositoryMock
                .Setup(x => x.GetPagedAsync(1, 10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);
            var query = new GetCurrenciesQuery(1, 10);
            var result = await _handler.Handle(query, CancellationToken.None);
            Assert.Empty(result.Items);
            Assert.Equal(0, result.TotalCount);
        }
    }
}
