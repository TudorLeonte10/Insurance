using Insurance.Application.Abstractions;
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
    public class CreateRiskFactorConfigurationCommandHandlerTests
    {
        private readonly Mock<IRiskFactorConfigurationRepository> _repoMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly CreateRiskFactorConfigurationCommandHandler _handler;

        public CreateRiskFactorConfigurationCommandHandlerTests()
        {
            _repoMock = new Mock<IRiskFactorConfigurationRepository>();
            _uowMock = new Mock<IUnitOfWork>();

            _handler = new CreateRiskFactorConfigurationCommandHandler(
                _repoMock.Object,
                _uowMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Create_RiskFactor()
        {

            var dto = new CreateRiskFactorConfigurationDto
            {
                Level = RiskFactorLevel.County,
                ReferenceId = "CITY_FIRE",
                AdjustmentPercentage = 10
            };

            var command = new CreateRiskFactorConfigurationCommand(dto);

            var id = await _handler.Handle(command, CancellationToken.None);

            Assert.NotEqual(Guid.Empty, id);

            _repoMock.Verify(
                x => x.AddAsync(
                    It.Is<Insurance.Domain.Metadata.RiskFactorConfiguration>(r =>
                        r.ReferenceId == "CITY_FIRE" &&
                        r.AdjustmentPercentage == 10),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _uowMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }

}
