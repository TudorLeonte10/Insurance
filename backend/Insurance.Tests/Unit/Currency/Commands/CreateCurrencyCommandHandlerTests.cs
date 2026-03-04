using Insurance.Application.Abstractions;
using Insurance.Application.Metadata.Currency.Commands;
using Insurance.Application.Metadata.Currency.DTOs;
using Insurance.Domain.Metadata;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.Currency.Commands
{
    public class CreateCurrencyCommandHandlerTests
    {
        private readonly Mock<ICurrencyRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CreateCurrencyCommandHandler _handler;

        public CreateCurrencyCommandHandlerTests()
        {
            _repositoryMock = new Mock<ICurrencyRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _handler = new CreateCurrencyCommandHandler(
                _repositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Create_Currency_And_Return_Id()
        {
            var dto = new CreateCurrencyDto
            {
                Code = "RON",
                Name = "Romanian Leu",
                ExchangeRateToBase = 1
            };

            var command = new CreateCurrencyCommand(dto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotEqual(Guid.Empty, result);

            _repositoryMock.Verify(x =>
                x.AddAsync(It.Is<Domain.Metadata.Currency>(c => c.IsActive), It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWorkMock.Verify(x =>
                x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

    }
}
