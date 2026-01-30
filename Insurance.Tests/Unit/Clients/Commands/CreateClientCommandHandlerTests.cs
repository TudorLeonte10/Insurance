using AutoMapper;
using Insurance.Application.Abstractions;
using Insurance.Application.Clients.Commands;
using Insurance.Application.Clients.Commands.CreateClient;
using Insurance.Application.Clients.DTOs;
using Insurance.Domain.Clients;
using Insurance.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.Unit.Clients.Commands
{
    public class CreateClientCommandHandlerTests
    {
        private readonly Mock<IClientRepository> _clientRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

        private readonly CreateClientCommandHandler _handler;

        public CreateClientCommandHandlerTests()
        {
            _handler = new CreateClientCommandHandler(
                _clientRepositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Given_ValidClient_Should_CreateClientSuccessfully()
        {
            var dto = new CreateClientDto
            {
                Name = "Valid Client",
                IdentificationNumber = "123456789",
                Email = "valid@test.ro",
                PhoneNumber = "0712345678",
                Type = ClientType.Individual,
                Address = "Str. Validului 1"
            };

            _clientRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new CreateClientCommand(dto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotEqual(Guid.Empty, result);

            _clientRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Given_DbConflict_Should_PropagateException()
        {
            var dto = new CreateClientDto
            {
                Name = "Client",
                IdentificationNumber = "123",
                Email = "x@test.ro",
                PhoneNumber = "0700000000",
                Type = ClientType.Individual
            };

            _clientRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DbUpdateException());

            var command = new CreateClientCommand(dto);

            await Assert.ThrowsAsync<DbUpdateException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }


    }
}
