using AutoMapper;
using Insurance.Application.Abstractions;
using Insurance.Application.Clients.Commands;
using Insurance.Application.Clients.Commands.CreateClient;
using Insurance.Application.Clients.DTOs;
using Insurance.Domain.Abstractions.Repositories;
using Insurance.Domain.Clients;
using Insurance.Domain.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.Clients.Commands
{
    public class UpdateClientCommandHandlerTests
    {
        private readonly Mock<IClientRepository> _clientRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        private readonly UpdateClientCommandHandler _handler;

        public UpdateClientCommandHandlerTests()
        {
            _handler = new UpdateClientCommandHandler(
                _clientRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Given_NonExistingClient_Should_ThrowNotFoundException()
        {
            var clientId = Guid.NewGuid();

            _clientRepositoryMock
                .Setup(r => r.GetByIdAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Client)null!);

            var command = new UpdateClientCommand(
                clientId,
                new UpdateClientDto { Name = "New Name" });

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));

            _unitOfWorkMock.Verify(
                u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Given_ExistingClient_Should_UpdateAndSave()
        {
            var clientId = Guid.NewGuid();
            var client = new Client { Id = clientId, Name = "Old Name" };

            _clientRepositoryMock
                .Setup(r => r.GetByIdAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);

            _mapperMock
                .Setup(m => m.Map(It.IsAny<UpdateClientDto>(), It.IsAny<Client>()))
                .Returns((UpdateClientDto dto, Client c) =>
                {
                    c.Name = dto.Name;
                    c.Email = dto.Email;
                    c.PhoneNumber = dto.PhoneNumber;
                    return c;
                });

            var command = new UpdateClientCommand(
                clientId,
                new UpdateClientDto { Name = "Updated Name" });

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(clientId, result);
            Assert.Equal("Updated Name", client.Name);

            _unitOfWorkMock.Verify(
                u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }


    }
}
