using Insurance.Application.Abstractions;
using Insurance.Application.Exceptions;
using Insurance.Application.FeeConfiguration.Command;
using Insurance.Application.FeeConfiguration.DTOs;
using Insurance.Domain.Metadata;
using Insurance.Domain.Metadata.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.FeeConfiguration.Commands
{
    public class UpdateFeeConfigurationCommandHandlerTests
    {
        private readonly Mock<IFeeConfigurationRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly UpdateFeeConfigurationCommandHandler _handler;

        public UpdateFeeConfigurationCommandHandlerTests()
        {
            _repositoryMock = new Mock<IFeeConfigurationRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _handler = new UpdateFeeConfigurationCommandHandler(
                _repositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Update_FeeConfiguration()
        {
            var id = Guid.NewGuid();

            var existing = new Domain.Metadata.FeeConfiguration
            {
                Id = id,
                Name = "Old name",
                Type = FeeType.AdminFee,
                Percentage = 0.01m,
                EffectiveFrom = DateTime.Today.AddDays(-10),
                IsActive = true
            };

            _repositoryMock
                .Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existing);

            var dto = new UpdateFeeConfigurationDto
            {
                Name = "New name",
                Type = FeeType.AdminFee,
                Percentage = 0.05m,
                EffectiveFrom = DateTime.Today,
                EffectiveTo = DateTime.Today.AddDays(5),
                IsActive = false
            };

            var command = new UpdateFeeConfigurationCommand(id, dto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(id, result);
            Assert.Equal("New name", existing.Name);
            Assert.Equal(0.05m, existing.Percentage);
            Assert.False(existing.IsActive);

            _repositoryMock.Verify(x =>
                x.UpdateAsync(existing, It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWorkMock.Verify(x =>
                x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFound_When_FeeConfiguration_Does_Not_Exist()
        {
            var id = Guid.NewGuid();

            _repositoryMock
                .Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Metadata.FeeConfiguration?)null);

            var command = new UpdateFeeConfigurationCommand(
                id,
                new UpdateFeeConfigurationDto());

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        
    }

}

