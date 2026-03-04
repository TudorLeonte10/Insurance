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
    public class RejectPolicyCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldRejectPolicy_AndSave()
        {
            var policy = PolicyDomainTests.CreateDraftPolicy();
            policy.SetToReview();

            var repo = new Mock<IPolicyRepository>();
            repo.Setup(r => r.GetByIdAsync(policy.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(policy);

            var uow = new Mock<IUnitOfWork>();
            var eventPublisher = new Mock<IIntegrationEventPublisher>();

            var handler = new RejectPolicyCommandHandler(
                repo.Object,
                uow.Object,
                eventPublisher.Object);

            var result = await handler.Handle(
                new RejectPolicyCommand(policy.Id),
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

            var handler = new RejectPolicyCommandHandler(
                repo.Object,
                Mock.Of<IUnitOfWork>(),
                Mock.Of<IIntegrationEventPublisher>());

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(
                    new RejectPolicyCommand(Guid.NewGuid()),
                    CancellationToken.None));
        }
    }
}
