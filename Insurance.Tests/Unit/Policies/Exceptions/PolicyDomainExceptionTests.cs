using Insurance.Domain.Exceptions;
using Insurance.Tests.Unit.Policy.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.Policies.Exceptions
{
    public class PolicyDomainExceptionTests
    {
        [Fact]
        public void CreateDraft_WithNegativeBasePremium_ShouldThrowInvalidBasePremiumException()
        {
            var clientId = Guid.NewGuid();
            var buildingId = Guid.NewGuid();
            var brokerId = Guid.NewGuid();
            var currencyId = Guid.NewGuid();
            var startDate = DateTime.UtcNow.AddDays(1);
            var endDate = DateTime.UtcNow.AddYears(1);
            var basePremium = -100m; 
            var finalPremium = 150m;
            var policyNumber = "POL-TEST-001";
            var now = DateTime.UtcNow;

            var exception = Assert.Throws<InvalidBasePremiumException>(() =>
                Domain.Policies.Policy.CreateDraft(
                    clientId,
                    buildingId,
                    brokerId,
                    currencyId,
                    basePremium,
                    finalPremium,
                    startDate,
                    endDate,
                    policyNumber,
                    now));

            Assert.NotNull(exception.Message);
        }

        [Fact]
        public void CreateDraft_WithZeroBasePremium_ShouldThrowInvalidBasePremiumException()
        {
            var clientId = Guid.NewGuid();
            var buildingId = Guid.NewGuid();
            var brokerId = Guid.NewGuid();
            var currencyId = Guid.NewGuid();
            var startDate = DateTime.UtcNow.AddDays(1);
            var endDate = DateTime.UtcNow.AddYears(1);
            var basePremium = 0m; 
            var finalPremium = 150m;
            var policyNumber = "POL-TEST-002";
            var now = DateTime.UtcNow;

            var exception = Assert.Throws<InvalidBasePremiumException>(() =>
                Domain.Policies.Policy.CreateDraft(
                    clientId,
                    buildingId,
                    brokerId,
                    currencyId,
                    basePremium,
                    finalPremium,
                    startDate,
                    endDate,
                    policyNumber,
                    now));

            Assert.NotNull(exception.Message);
        }

        [Fact]
        public void CreateDraft_WithFinalPremiumLessThanBasePremium_ShouldThrowInvalidFinalPremiumException()
        {
            var clientId = Guid.NewGuid();
            var buildingId = Guid.NewGuid();
            var brokerId = Guid.NewGuid();
            var currencyId = Guid.NewGuid();
            var startDate = DateTime.UtcNow.AddDays(1);
            var endDate = DateTime.UtcNow.AddYears(1);
            var basePremium = 100m;
            var finalPremium = 50m; 
            var policyNumber = "POL-TEST-003";
            var now = DateTime.UtcNow;

            var exception = Assert.Throws<InvalidFinalPremiumException>(() =>
                Domain.Policies.Policy.CreateDraft(
                    clientId,
                    buildingId,
                    brokerId,
                    currencyId,
                    basePremium,
                    finalPremium,
                    startDate,
                    endDate,
                    policyNumber,
                    now));

            Assert.NotNull(exception.Message);
        }

        [Fact]
        public void CreateDraft_WithEndDateBeforeStartDate_ShouldThrowInvalidPolicyTermException()
        {
            var clientId = Guid.NewGuid();
            var buildingId = Guid.NewGuid();
            var brokerId = Guid.NewGuid();
            var currencyId = Guid.NewGuid();
            var startDate = DateTime.UtcNow.AddYears(1);
            var endDate = DateTime.UtcNow.AddDays(1); 
            var basePremium = 100m;
            var finalPremium = 150m;
            var policyNumber = "POL-TEST-004";
            var now = DateTime.UtcNow;

            var exception = Assert.Throws<InvalidPolicyTermException>(() =>
                Domain.Policies.Policy.CreateDraft(
                    clientId,
                    buildingId,
                    brokerId,
                    currencyId,
                    basePremium,
                    finalPremium,
                    startDate,
                    endDate,
                    policyNumber,
                    now));

            Assert.NotNull(exception.Message);
        }

        [Fact]
        public void Cancel_WhenAlreadyCancelled_ShouldThrowInvalidPolicyTransitionException()
        {
          
            var policy = PolicyDomainTests.CreateActivePolicy();
            policy.Cancel("First cancellation");

           
            var exception = Assert.Throws<InvalidPolicyTransitionException>(() =>
                policy.Cancel("Second cancellation")); 

            Assert.NotNull(exception.Message);
        }

        [Fact]
        public void Cancel_WhenInDraftStatus_ShouldThrowInvalidPolicyTransitionException()
        {
         
            var policy = PolicyDomainTests.CreateDraftPolicy();

           
            var exception = Assert.Throws<InvalidPolicyTransitionException>(() =>
                policy.Cancel("Cancellation")); 

            Assert.NotNull(exception.Message);
        }
    }
}
