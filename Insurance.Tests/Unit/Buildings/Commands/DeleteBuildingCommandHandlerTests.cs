using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Insurance.Application.Clients.Commands;
using Insurance.Domain.Clients;
using Insurance.Domain.Exceptions;
using Insurance.Domain.Abstractions.Repositories;
using Insurance.Application.Abstractions;

namespace Insurance.Tests.Unit.Clients.Commands
{
    public class DeleteClientCommandHandlerTests
    {
        private readonly Mock<IClientRepository> _clientRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _uowMock = new();

        private readonly DeleteClientCommandHandler _handler;

        public DeleteClientCommandHandlerTests()
        {
            _handler = new DeleteClientCommandHandler(
                _clientRepositoryMock.Object,
                _uowMock.Object);
        }

        private static DeleteClientCommand CreateCommand(Guid clientId) => new DeleteClientCommand(clientId);

        private static Client CreateClient() => Client.Create(
            ClientType.Individual,
            "X",
            "ID",
            "x@test.ro",
            "0712345678",
            "Str. Test 1"
        );


        [Fact]
        public async Task Given_NonExistingClient_Should_ThrowNotFoundException()
        {

            var clientId = Guid.NewGuid();

            _clientRepositoryMock
                .Setup(r => r.GetByIdAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Client)null!);

            var command = CreateCommand(clientId);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));

            _clientRepositoryMock.Verify(
                r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),Times.Never);

            _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Given_ExistingClient_Should_DeleteAndSave()
        {
            var client = CreateClient();
            var clientId = client.Id;

            _clientRepositoryMock
                .Setup(r => r.GetByIdAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);

            var command = CreateCommand(clientId);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(clientId, result);

            _clientRepositoryMock.Verify(
                r => r.DeleteAsync(clientId, It.IsAny<CancellationToken>()), Times.Once);

            _uowMock.Verify(
                u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
