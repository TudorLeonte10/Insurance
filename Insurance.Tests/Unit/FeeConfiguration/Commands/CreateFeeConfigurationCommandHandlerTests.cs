using Insurance.Application.Abstractions;
using Insurance.Application.FeeConfiguration.Command;
using Insurance.Application.FeeConfiguration.DTOs;
using Insurance.Domain.Metadata;
using Insurance.Domain.Metadata.Enums;
using Insurance.Infrastructure.Persistence.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.FeeConfiguration.Commands
{
    public class CreateFeeConfigurationCommandHandlerTests
    {
        private readonly Mock<IFeeConfigurationRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CreateFeeConfigurationCommandHandler _handler;

        public CreateFeeConfigurationCommandHandlerTests()
        {
            _repositoryMock = new Mock<IFeeConfigurationRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _handler = new CreateFeeConfigurationCommandHandler(
                _repositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Create_FeeConfiguration_And_Return_Id()
        {
            var dto = new CreateFeeConfigurationDto
            {
                Name = "Admin fee",
                Type = FeeType.AdminFee,
                Percentage = 0.05m,
                EffectiveFrom = DateTime.Today,
                EffectiveTo = DateTime.Today.AddDays(10)
            };

            var command = new CreateFeeConfigurationCommand(dto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotEqual(Guid.Empty, result);

            _repositoryMock.Verify(x =>
                x.AddAsync(It.IsAny<Domain.Metadata.FeeConfiguration>(), It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWorkMock.Verify(x =>
                x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }


        [Fact]
        public async Task Handle_Should_Pass_Correct_Entity_To_Repository()
        {
            var dto = new CreateFeeConfigurationDto
            {
                Name = "Processing fee",
                Type = FeeType.AdminFee,
                Percentage = 0.02m,
                EffectiveFrom = DateTime.Today,
                EffectiveTo = DateTime.Today.AddDays(15)
            };
            Domain.Metadata.FeeConfiguration? passedEntity = null;
            _repositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Domain.Metadata.FeeConfiguration>(), It.IsAny<CancellationToken>()))
                .Callback<Domain.Metadata.FeeConfiguration, CancellationToken>((entity, _) =>
                {
                    passedEntity = entity;
                })
                .Returns(Task.CompletedTask);
            var command = new CreateFeeConfigurationCommand(dto);
            await _handler.Handle(command, CancellationToken.None);
            Assert.NotNull(passedEntity);
            Assert.Equal(dto.Name, passedEntity!.Name);
            Assert.Equal(dto.Type, passedEntity.Type);
            Assert.Equal(dto.Percentage, passedEntity.Percentage);
            Assert.Equal(dto.EffectiveFrom, passedEntity.EffectiveFrom);
            Assert.Equal(dto.EffectiveTo, passedEntity.EffectiveTo);
        }

    }

}
