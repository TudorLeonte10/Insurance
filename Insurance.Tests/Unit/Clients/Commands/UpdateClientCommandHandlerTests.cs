using AutoMapper;
using Insurance.Application.Abstractions;
using Insurance.Application.Abstractions.Audit;
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
        private readonly Mock<IAuditLogService> _auditLogServiceMock = new();

        private readonly UpdateClientCommandHandler _handler;

        public UpdateClientCommandHandlerTests()
        {
            _handler = new UpdateClientCommandHandler(
                _clientRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _auditLogServiceMock.Object);
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
            var client = Client.Create(
                ClientType.Individual,
                "Old Name",
                "1234567890123",
                "old@test.ro",
                "0712345678",
                "Str. Test 1"
            );

            var clientId = client.Id;

            _clientRepositoryMock
                .Setup(r => r.GetByIdAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);

            var command = new UpdateClientCommand(
                clientId,
                new UpdateClientDto
                {
                    Name = "Updated Name",
                    Email = "new@test.ro",
                    PhoneNumber = "0799999999",
                    Address = "Str. Noua 2",
                    IdentificationNumber = "1234567890123" 
                });

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(clientId, result);
            Assert.Equal("Updated Name", client.Name);
            Assert.Equal("new@test.ro", client.Email);
            Assert.Equal("0799999999", client.PhoneNumber);
            Assert.Equal("Str. Noua 2", client.Address);

            _unitOfWorkMock.Verify(
                u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

    }
}
