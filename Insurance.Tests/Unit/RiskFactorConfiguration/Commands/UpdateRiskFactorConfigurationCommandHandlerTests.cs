using Insurance.Application.Abstractions;
using Insurance.Application.Exceptions;
using Insurance.Application.Metadata.RiskFactors.Commands;
using Insurance.Application.Metadata.RiskFactors.DTOs;
using Insurance.Domain.Metadata;
using Insurance.Domain.Metadata.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.RiskFactorConfiguration.Commands
{
    public class UpdateRiskFactorConfigurationCommandHandlerTests
    {
        private readonly Mock<IRiskFactorConfigurationRepository> _repoMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly UpdateRiskFactorConfigurationCommandHandler _handler;

        public UpdateRiskFactorConfigurationCommandHandlerTests()
        {
            _repoMock = new Mock<IRiskFactorConfigurationRepository>();
            _uowMock = new Mock<IUnitOfWork>();

            _handler = new UpdateRiskFactorConfigurationCommandHandler(
                _repoMock.Object,
                _uowMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Update_RiskFactor_When_Found()
        {
            var existing = new Insurance.Domain.Metadata.RiskFactorConfiguration
            {
                Id = Guid.NewGuid(),
                Level = RiskFactorLevel.County,
                ReferenceId = "OLD",
                AdjustmentPercentage = 5,
                IsActive = true
            };

            _repoMock
                .Setup(x => x.GetByIdAsync(existing.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existing);

            var dto = new UpdateRiskFactorConfigurationDto
            {
                Level = RiskFactorLevel.City,
                ReferenceId = "NEW",
                AdjustmentPercentage = 20
            };

            var command = new UpdateRiskFactorConfigurationCommand(dto, existing.Id);

            await _handler.Handle(command, CancellationToken.None);

            Assert.Equal("NEW", existing.ReferenceId);
            Assert.Equal(20, existing.AdjustmentPercentage);

            _uowMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFound_When_Not_Exists()
        {
            _repoMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Insurance.Domain.Metadata.RiskFactorConfiguration?)null);

            var command = new UpdateRiskFactorConfigurationCommand(
                new UpdateRiskFactorConfigurationDto(), Guid.NewGuid());

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }

}
