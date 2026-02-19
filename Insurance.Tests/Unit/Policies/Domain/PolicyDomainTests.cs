using Insurance.Domain.Exceptions;
using Insurance.Domain.Policies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.Policy.Domain
{
    public class PolicyDomainTests
    {
        [Fact]
        public void Activate_DraftPolicy_ShouldBecomeActive()
        {
            var policy = CreateDraftPolicy();

            policy.Activate(DateTime.UtcNow);

            Assert.Equal(PolicyStatus.Active, policy.Status);
            Assert.NotNull(policy.ActivatedAt);
        }

        [Fact]
        public void Activate_WhenNotDraft_ShouldThrow()
        {
            var policy = CreateActivePolicy();

            Assert.Throws<InvalidPolicyTransitionException>(() =>
                policy.Activate(DateTime.UtcNow));
        }

        [Fact]
        public void Activate_WhenStartDateInPast_ShouldThrow()
        {
            var policy = CreateDraftPolicyWithStartDate(DateTime.UtcNow.AddDays(-1));


            Assert.Throws<InvalidPolicyTransitionException>(() =>
                policy.Activate(DateTime.UtcNow));
        }


        [Fact]
        public void Cancel_ActivePolicy_ShouldBecomeCancelled()
        {
            var policy = CreateActivePolicy();

            policy.Cancel("client request", DateTime.UtcNow);

            Assert.Equal(PolicyStatus.Cancelled, policy.Status);
            Assert.Equal("client request", policy.CancellationReason);
        }

        [Fact]
        public void Cancel_WhenNotActive_ShouldThrow()
        {
            var policy = CreateDraftPolicy();

            Assert.Throws<InvalidPolicyTransitionException>(() =>
                policy.Cancel("reason", DateTime.UtcNow));
        }

        public static Insurance.Domain.Policies.Policy CreateDraftPolicy() =>
    Insurance.Domain.Policies.Policy.CreateDraft(
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        100,
        120,
        DateTime.UtcNow.AddDays(1),
        DateTime.UtcNow.AddDays(10),
        "POL-1",
        DateTime.UtcNow);

        public static Insurance.Domain.Policies.Policy CreateActivePolicy()
        {
            var policy = CreateDraftPolicy();
            policy.Activate(DateTime.UtcNow);
            return policy;
        }

        public static Insurance.Domain.Policies.Policy CreateDraftPolicyWithStartDate(DateTime startDate) =>
            Insurance.Domain.Policies.Policy.CreateDraft(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                100,
                120,
                startDate,
                DateTime.UtcNow.AddDays(10),
                "POL-1",
                DateTime.UtcNow);
    }
}
