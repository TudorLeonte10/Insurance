using AutoMapper;
using FluentAssertions;
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
    public class CreateClientCommandHandlerTests
    {
        private readonly Mock<IClientRepository> _clientRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly CreateClientCommandHandler _handler;

        public CreateClientCommandHandlerTests()
        {
            _clientRepositoryMock = new Mock<IClientRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _handler = new CreateClientCommandHandler(
                _clientRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task Given_DuplicateIdentificationNumber_Should_ThrowConflictException()
        {
            var dto = new CreateClientDto
            {
                Name = "Test Client",
                IdentificationNumber = "123456789",
                Email = "test@test.ro",
                PhoneNumber = "0712345678",
                Type = ClientType.Individual
            };

            var command = new CreateClientCommand(dto);

            _clientRepositoryMock
                .Setup(x => x.ExistsByIdentifierAsync(dto.IdentificationNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            Func<Task> act = async () =>
                await _handler.Handle(command, CancellationToken.None);

            await act.Should()
                .ThrowAsync<ConflictException>();
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
                Type = ClientType.Individual
            };

            var client = new Client { Id = Guid.NewGuid() };

            _clientRepositoryMock
                .Setup(x => x.ExistsByIdentifierAsync(dto.IdentificationNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(x => x.Map<Client>(dto))
                .Returns(client);

            var command = new CreateClientCommand(dto);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().Be(client.Id);

            _clientRepositoryMock.Verify(x => x.AddAsync(client, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
