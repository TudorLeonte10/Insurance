using Insurance.Application.Abstractions;
using Insurance.Application.Abstractions.Messaging;
using Insurance.Application.Authentication;
using Insurance.Application.Exceptions;
using Insurance.Application.Policy.Commands;
using Insurance.Domain.Exceptions;
using Insurance.Domain.Policies;
using Insurance.Tests.Unit.Policy.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.Policy.Commands
{
    public class ActivatePolicyCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldActivatePolicy_AndSave()
        {
            var policy = PolicyDomainTests.CreateDraftPolicy();

            var currentUser = new Mock<ICurrentUserContext>();
            currentUser.SetupGet(c => c.BrokerId).Returns(policy.BrokerId);
            var repo = new Mock<IPolicyRepository>();
            var eventus = new Mock<IIntegrationEventPublisher>();
            repo.Setup(r => r.GetByIdAsync(policy.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(policy);

            var uow = new Mock<IUnitOfWork>();

            var handler = new ActivatePolicyCommandHandler(
                repo.Object,
                uow.Object,
                currentUser.Object,
                eventus.Object);

            await handler.Handle(
                new ActivatePolicyCommand(policy.Id),
                CancellationToken.None);

            repo.Verify(r =>
                r.UpdateAsync(policy, It.IsAny<CancellationToken>()),
                Times.Once);

            uow.Verify(u =>
                u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_WhenPolicyNotFound_ShouldThrow()
        {
            var currentUser = new Mock<ICurrentUserContext>();
            var repo = new Mock<IPolicyRepository>();
            repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Insurance.Domain.Policies.Policy?)null);

            var handler = new ActivatePolicyCommandHandler(
                repo.Object,
                Mock.Of<IUnitOfWork>(),
                currentUser.Object,
                Mock.Of<IIntegrationEventPublisher>());

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(
                    new ActivatePolicyCommand(Guid.NewGuid()),
                    CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WhenPolicyIsNotDraft_ShouldThrow()
        {
            var policy = PolicyDomainTests.CreateActivePolicy();

            var currentUser = new Mock<ICurrentUserContext>();
            currentUser.SetupGet(c => c.BrokerId).Returns(policy.BrokerId);

            var repo = new Mock<IPolicyRepository>();
            repo.Setup(r => r.GetByIdAsync(policy.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(policy);

            var handler = new ActivatePolicyCommandHandler(
                repo.Object,
                Mock.Of<IUnitOfWork>(),
                currentUser.Object,
                Mock.Of<IIntegrationEventPublisher>());

            await Assert.ThrowsAsync<InvalidPolicyTransitionException>(() =>
                handler.Handle(
                    new ActivatePolicyCommand(policy.Id),
                    CancellationToken.None));
        }

    }
}
