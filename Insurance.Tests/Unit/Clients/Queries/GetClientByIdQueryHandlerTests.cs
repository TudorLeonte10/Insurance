using AutoMapper;
using Insurance.Application.Clients.DTOs;
using Insurance.Application.Clients.Queries;
using Insurance.Domain.Abstractions.Repositories;
using Insurance.Domain.Clients;
using Insurance.Domain.Exceptions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.Unit.Clients.Queries
{
    public class GetClientByIdQueryHandlerTests
    {
        private readonly Mock<IClientRepository> _clientRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        private readonly GetClientByIdQueryHandler _handler;

        public GetClientByIdQueryHandlerTests()
        {
            _handler = new GetClientByIdQueryHandler(
                _clientRepositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Given_ExistingClient_Should_ReturnClientDetailsDto()
        {
            var clientId = Guid.NewGuid();

            var client = new Client
            {
                Id = clientId,
                Name = "Test Client"
            };

            var dto = new ClientDetailsDto
            {
                Id = clientId,
                Name = "Test Client"
            };

            _clientRepositoryMock
                .Setup(r => r.GetByIdAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);

            _mapperMock
                .Setup(m => m.Map<ClientDetailsDto>(client))
                .Returns(dto);

            var query = new GetClientByIdQuery(clientId);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(clientId, result.Id);
            Assert.Equal("Test Client", result.Name);
        }

        [Fact]
        public async Task Given_NonExistingClient_Should_ThrowNotFoundException()
        {
            var clientId = Guid.NewGuid();

            _clientRepositoryMock
                .Setup(r => r.GetByIdAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Client)null!);

            var query = new GetClientByIdQuery(clientId);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
    }
}
