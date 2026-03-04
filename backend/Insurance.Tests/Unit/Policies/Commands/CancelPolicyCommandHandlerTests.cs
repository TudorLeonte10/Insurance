using Insurance.Application.Abstractions;
using Insurance.Application.Abstractions.Messaging;
using Insurance.Application.Authentication;
using Insurance.Application.Exceptions;
using Insurance.Application.Policy.Commands;
using Insurance.Domain.Policies;
using Insurance.Tests.Unit.Policy.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.Policy.Commands
{
    public class CancelPolicyCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldCancelPolicy_WithReason_AndSave()
        {
            var policy = PolicyDomainTests.CreateActivePolicy();
            var cancellationReason = "Customer request";

            var currentUser = new Mock<ICurrentUserContext>();
            currentUser.SetupGet(c => c.BrokerId).Returns(policy.BrokerId);

            var repo = new Mock<IPolicyRepository>();
            repo.Setup(r => r.GetByIdAsync(policy.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(policy);

            var uow = new Mock<IUnitOfWork>();

            var handler = new CancelPolicyCommandHandler(
                repo.Object,
                uow.Object,
                currentUser.Object,
                Mock.Of<IIntegrationEventPublisher>(),
                Mock.Of<TimeProvider>());

            await handler.Handle(
                new CancelPolicyCommand(policy.Id, cancellationReason),
                CancellationToken.None);

            repo.Verify(r =>
                r.UpdateAsync(policy, It.IsAny<CancellationToken>()),
                Times.Once);

            uow.Verify(u =>
                u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldCancelPolicy_WithoutReason_AndSave()
        {
            var policy = PolicyDomainTests.CreateActivePolicy();

            var currentUser = new Mock<ICurrentUserContext>();
            currentUser.SetupGet(c => c.BrokerId).Returns(policy.BrokerId);

            var repo = new Mock<IPolicyRepository>();
            repo.Setup(r => r.GetByIdAsync(policy.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(policy);

            var uow = new Mock<IUnitOfWork>();

            var handler = new CancelPolicyCommandHandler(
                repo.Object,
                uow.Object,
                currentUser.Object,
                Mock.Of<IIntegrationEventPublisher>(),
                Mock.Of<TimeProvider>());

            await handler.Handle(
                new CancelPolicyCommand(policy.Id, null),
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

            var handler = new CancelPolicyCommandHandler(
                repo.Object,
                Mock.Of<IUnitOfWork>(),
                currentUser.Object,
                Mock.Of<IIntegrationEventPublisher>(),
                Mock.Of<TimeProvider>());

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(
                    new CancelPolicyCommand(Guid.NewGuid(), "Test reason"),
                    CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldSetCancellationReason_WhenProvided()
        {
            var policy = PolicyDomainTests.CreateActivePolicy();
            var cancellationReason = "Duplicate policy";

            var currentUser = new Mock<ICurrentUserContext>();
            currentUser.SetupGet(c => c.BrokerId).Returns(policy.BrokerId);

            var repo = new Mock<IPolicyRepository>();
            repo.Setup(r => r.GetByIdAsync(policy.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(policy);

            var uow = new Mock<IUnitOfWork>();

            var handler = new CancelPolicyCommandHandler(
                repo.Object,
                uow.Object,
                currentUser.Object,
                Mock.Of<IIntegrationEventPublisher>(),
                Mock.Of<TimeProvider>());

            await handler.Handle(
                new CancelPolicyCommand(policy.Id, cancellationReason),
                CancellationToken.None);

            Assert.Equal(PolicyStatus.Cancelled, policy.Status);
            Assert.Equal(cancellationReason, policy.CancellationReason);
            Assert.NotNull(policy.CancelledAt);
        }
    }
}
