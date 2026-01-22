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
    public class GetClientsQueryHandlerTests
    {
        private readonly Mock<IClientRepository> _clientRepositoryMock = new();

        private readonly GetClientsQueryHandler _handler;

        public GetClientsQueryHandlerTests()
        {
            _handler = new GetClientsQueryHandler(_clientRepositoryMock.Object);
        }

        [Fact]
        public async Task Given_ValidQuery_Should_ReturnPagedResult()
        {
            var expectedResult = new PagedResult<ClientDetailsDto>(
                items: new[]
                {
                    new ClientDetailsDto { Name = "Client 1" },
                    new ClientDetailsDto { Name = "Client 2" }
                },
                pageNumber: 1,
                pageSize: 10,
                totalCount: 2
            );

            _clientRepositoryMock
                .Setup(r => r.GetPagedAsync(
                    1,
                    10,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var query = new GetClientsQuery(pageNumber: 1, pageSize: 10);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.Equal(2, result.TotalCount);
        }
    }
}

