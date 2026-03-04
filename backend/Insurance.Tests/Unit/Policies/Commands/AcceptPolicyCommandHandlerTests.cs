using Insurance.Application.Abstractions;
using Insurance.Application.Abstractions.Messaging;
using Insurance.Application.Exceptions;
using Insurance.Application.Policy.Commands;
using Insurance.Domain.Policies;
using Insurance.Tests.Unit.Policy.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.Policies.Commands
{
    public class AcceptPolicyCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldSetToDraft_AndSave()
        {
            // create a policy and put it UnderReview (Accept operates on policies under review)
            var policy = PolicyDomainTests.CreateDraftPolicy();
            policy.SetToReview();

            var repo = new Mock<IPolicyRepository>();
            repo.Setup(r => r.GetByIdAsync(policy.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(policy);

            var uow = new Mock<IUnitOfWork>();
            var eventPublisher = new Mock<IIntegrationEventPublisher>();

            var handler = new AcceptPolicyCommandHandler(
                repo.Object,
                uow.Object,
                eventPublisher.Object);

            var result = await handler.Handle(
                new AcceptPolicyCommand(policy.Id),
                CancellationToken.None);

            Assert.Equal(policy.Id, result);

            repo.Verify(r =>
                r.UpdateAsync(policy, It.IsAny<CancellationToken>()),
                Times.Once);

            eventPublisher.Verify(e =>
                e.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()),
                Times.Once);

            uow.Verify(u =>
                u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_WhenPolicyNotFound_ShouldThrow()
        {
            var repo = new Mock<IPolicyRepository>();
            repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Policies.Policy?)null);

            var handler = new AcceptPolicyCommandHandler(
                repo.Object,
                Mock.Of<IUnitOfWork>(),
                Mock.Of<IIntegrationEventPublisher>());

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(
                    new AcceptPolicyCommand(Guid.NewGuid()),
                    CancellationToken.None));
        }
    }
}

