using AutoMapper;
using Insurance.Application.Clients.DTOs;
using Insurance.Application.Clients.Queries;
using Insurance.Application.Common.Paging;
using Insurance.Domain.Abstractions.Repositories;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.Unit.Clients.Queries
{
    public class SearchClientsQueryHandlerTests
    {
        private readonly Mock<IClientRepository> _clientRepositoryMock = new();

        private readonly SearchClientsQueryHandler _handler;

        public SearchClientsQueryHandlerTests()
        {
            _handler = new SearchClientsQueryHandler(_clientRepositoryMock.Object);
        }

        [Fact]
        public async Task Given_SearchCriteria_Should_ReturnPagedResult()
        {
            var expectedResult = new PagedResult<ClientDetailsDto>(
                items: new[]
                {
                    new ClientDetailsDto { Name = "John Doe" }
                },
                pageNumber: 1,
                pageSize: 10,
                totalCount: 1
            );

            _clientRepositoryMock
                .Setup(r => r.SearchAsync(
                    "John",
                    "123",
                    1,
                    10,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var query = new SearchClientsQuery(
                name: "John",
                identificationNumber: "123",
                pageNumber: 1,
                pageSize: 10);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal(1, result.TotalCount);
        }
    }
}
