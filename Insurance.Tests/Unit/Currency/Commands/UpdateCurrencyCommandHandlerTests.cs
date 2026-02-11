using Insurance.Application.Abstractions;
using Insurance.Application.Exceptions;
using Insurance.Application.Metadata.Currency.Commands;
using Insurance.Application.Metadata.Currency.DTOs;
using Insurance.Domain.Metadata;
using Moq;


namespace Insurance.Tests.Unit.Currency.Commands
{
    public class UpdateCurrencyCommandHandlerTests
    {
        private readonly Mock<ICurrencyRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly UpdateCurrencyCommandHandler _handler;

        public UpdateCurrencyCommandHandlerTests()
        {
            _repositoryMock = new Mock<ICurrencyRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _handler = new UpdateCurrencyCommandHandler(
                _repositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Update_Currency()
        {
            var id = Guid.NewGuid();

            var currency = new Domain.Metadata.Currency
            {
                Id = id,
                Code = "EUR",
                Name = "Euro",
                ExchangeRateToBase = 4.9m,
                IsActive = true
            };

            _repositoryMock
                .Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(currency);

            var dto = new UpdateCurrencyDto
            {
                Code = "EUR",
                Name = "Euro Updated",
                ExchangeRateToBase = 5.0m
            };

            var command = new UpdateCurrencyCommand(dto, id);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(id, result);

            _repositoryMock.Verify(x =>
                x.UpdateAsync(currency, It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWorkMock.Verify(x =>
                x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Currency_Not_Found()
        {
            _repositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Metadata.Currency?)null);

            var command = new UpdateCurrencyCommand(new UpdateCurrencyDto(), Guid.NewGuid());

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }

}

