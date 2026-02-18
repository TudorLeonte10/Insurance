using Insurance.Application.Abstractions;
using Insurance.Application.Abstractions.Messaging;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Authentication;
using Insurance.Application.Brokers;
using Insurance.Application.Buildings;
using Insurance.Application.Buildings.DTOs;
using Insurance.Application.Clients;
using Insurance.Application.Common;
using Insurance.Application.Exceptions;
using Insurance.Application.Policy.Commands;
using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Services;
using Insurance.Domain.Brokers;
using Insurance.Domain.Clients;
using Insurance.Domain.Metadata;
using Insurance.Domain.Policies;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.Unit.Polic.Commands
{
    public class CreatePolicyCommandHandlerTests
    {
        private readonly Mock<IPolicyCreationService> _creationService = new();
        private readonly Mock<IPolicyRepository> _policyRepo = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<ICurrentUserContext> _currentUserContext = new();
        private readonly Mock<IIntegrationEventPublisher> _eventPublisher = new();

        private readonly CreatePolicyCommandHandler _handler;

        public CreatePolicyCommandHandlerTests()
        {
            _handler = new CreatePolicyCommandHandler(
                _creationService.Object,
                _policyRepo.Object,
                _unitOfWork.Object,
                _currentUserContext.Object,
                _eventPublisher.Object);
        }

        [Fact]
        public async Task Handle_Should_Call_Service_And_Persist_Policy()
        {
            var brokerId = Guid.NewGuid();
            _currentUserContext.SetupGet(x => x.BrokerId).Returns(brokerId);

            var dto = new CreatePolicyDto
            {
                ClientId = Guid.NewGuid(),
                BuildingId = Guid.NewGuid(),
                CurrencyId = Guid.NewGuid(),
                BasePremium = 1000m,
                StartDate = DateTime.UtcNow.Date.AddDays(1),
                EndDate = DateTime.UtcNow.Date.AddMonths(12)
            };

            var command = new CreatePolicyCommand(dto);

          
            var now = DateTime.UtcNow;
            var policy = Domain.Policies.Policy.CreateDraft(
                clientId: dto.ClientId,
                buildingId: dto.BuildingId,
                brokerId: brokerId,
                currencyId: dto.CurrencyId,
                basePremium: dto.BasePremium,
                finalPremium: 1500m,
                startDate: dto.StartDate,
                endDate: dto.EndDate,
                policyNumber: $"POL-{Guid.NewGuid():N}",
                now: now);

            var creationResult = new PolicyCreationResult
            {
                Policy = policy,
                Country = "Romania",
                County = "SomeCounty",
                City = "SomeCity",
                BrokerCode = "BRK001",
                Currency = "RON",
                Status = policy.Status.ToString(),
                FinalPremium = policy.FinalPremium,
                FinalPremiumInBase = policy.FinalPremium, // adjust if needed
                CreatedAt = now
            };

            _creationService
                .Setup(s => s.CreatePolicyAsync(dto, brokerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(creationResult);


            _policyRepo
                .Setup(r => r.AddAsync(policy, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _unitOfWork
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

           
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(policy.Id, result);

            _creationService.Verify(s => s.CreatePolicyAsync(dto, brokerId, It.IsAny<CancellationToken>()), Times.Once);
            _policyRepo.Verify(r => r.AddAsync(policy, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Propagate_Exception_From_Service()
        {
            var brokerId = Guid.NewGuid();
            _currentUserContext.SetupGet(x => x.BrokerId).Returns(brokerId);

            var dto = new CreatePolicyDto
            {
                ClientId = Guid.NewGuid(),
                BuildingId = Guid.NewGuid(),
                CurrencyId = Guid.NewGuid(),
                BasePremium = 1000m,
                StartDate = DateTime.UtcNow.Date.AddDays(1),
                EndDate = DateTime.UtcNow.Date.AddMonths(12)
            };

            var command = new CreatePolicyCommand(dto);

            _creationService
                .Setup(s => s.CreatePolicyAsync(dto, brokerId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException("Client not found"));

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

            _policyRepo.Verify(r => r.AddAsync(It.IsAny<Domain.Policies.Policy>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
