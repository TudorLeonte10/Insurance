using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Clients.DTOs;
using Insurance.Application.Clients.Queries;
using Insurance.Application.Common.Paging;
using Insurance.Application.Exceptions;
using Insurance.Domain.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.Unit.Clients.Queries
{
    public class GetClientsQueryHandlerTests
    {
        private readonly Mock<IClientSearchRepository> _searchRepositoryMock = new();
        private readonly Mock<IClientReadRepository> _readRepositoryMock = new();

        private readonly GetClientsQueryHandler _handler;

        public GetClientsQueryHandlerTests()
        {
            _handler = new GetClientsQueryHandler(
                _searchRepositoryMock.Object,
                _readRepositoryMock.Object);
        }

        [Fact]
        public async Task Given_ClientId_When_ClientExists_Should_ReturnSingleItemPagedResult()
        {
            var clientId = Guid.NewGuid();

            var clientDto = new ClientDetailsDto
            {
                Id = clientId,
                Name = "Test Client"
            };

            _readRepositoryMock
                .Setup(r => r.GetByIdAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientDto);

            var query = new GetClientsQuery
            {
                ClientId = clientId,
                PageNumber = 1,
                PageSize = 10
            };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Single(result.Items);
            Assert.Equal(clientId, result.Items[0].Id);
            Assert.Equal(1, result.TotalCount);

            _readRepositoryMock.Verify(
                r => r.GetByIdAsync(clientId, It.IsAny<CancellationToken>()),
                Times.Once);

            _searchRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Given_ClientId_When_ClientDoesNotExist_Should_ThrowNotFoundException()
        { 
            var clientId = Guid.NewGuid();

            _readRepositoryMock
                .Setup(r => r.GetByIdAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ClientDetailsDto?)null);

            var query = new GetClientsQuery
            {
                ClientId = clientId
            };

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Given_NoClientId_Should_SearchClients()
        {
            var clients = new List<ClientDetailsDto>
            {
                new ClientDetailsDto { Id = Guid.NewGuid(), Name = "Client 1" },
                new ClientDetailsDto { Id = Guid.NewGuid(), Name = "Client 2" }
            };

            var pagedResult = new PagedResult<ClientDetailsDto>(
                clients,
                pageNumber: 1,
                pageSize: 10,
                totalCount: 2);

            _searchRepositoryMock
                .Setup(r => r.SearchAsync(
                    It.IsAny<string?>(),
                    It.IsAny<string?>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedResult);

            var query = new GetClientsQuery
            {
                Name = "Client",
                PageNumber = 1,
                PageSize = 10
            };

  
            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(2, result.Items.Count);
            Assert.Equal(2, result.TotalCount);

            _searchRepositoryMock.Verify(
                r => r.SearchAsync(
                    query.Name,
                    query.IdentificationNumber,
                    query.PageNumber,
                    query.PageSize,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _readRepositoryMock.VerifyNoOtherCalls();
        }
    }
}
