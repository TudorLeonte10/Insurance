using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Enums;
using Insurance.Application.Policy.Queries;
using Insurance.Domain.Buildings;
using Insurance.Domain.Policies;
using Insurance.Infrastructure.Reports;
using Insurance.Reporting.Infrastructure.Entities;
using Insurance.Reporting.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.Reporting
{
    public class PolicyReportGroupingTests
    {
        [Fact]
        public void CityReportGrouping_GroupsAndSumsCorrectly()
        {
            var now = DateTime.UtcNow;
            var items = new List<PolicyReportAggregate>
            {
                new() { PolicyId = Guid.NewGuid(), City = "Bucharest", Currency = "RON", FinalPremium = 100m, FinalPremiumInBase = 100m, CreatedAt = now, Status = "Active", BuildingType = "Residential" },
                new() { PolicyId = Guid.NewGuid(), City = "Bucharest", Currency = "RON", FinalPremium = 200m, FinalPremiumInBase = 200m, CreatedAt = now, Status = "Active", BuildingType = "Residential" },
                new() { PolicyId = Guid.NewGuid(), City = "Bucharest", Currency = "EUR", FinalPremium = 150m, FinalPremiumInBase = 330m, CreatedAt = now, Status = "Active", BuildingType = "Residential" },
                new() { PolicyId = Guid.NewGuid(), City = "Cluj", Currency = "RON", FinalPremium = 50m, FinalPremiumInBase = 50m, CreatedAt = now, Status = "Active", BuildingType = "Residential" },
            }.AsQueryable();

            var strategy = new CityReportGrouping();

            var result = strategy.Group(items).ToList();

            Assert.Equal(3, result.Count);

            var bucharestRon = result.Single(r => r.GroupName == "Bucharest" && r.Currency == "RON");
            Assert.Equal(2, bucharestRon.PoliciesCount);
            Assert.Equal(300m, bucharestRon.TotalPremium);
            Assert.Equal(300m, bucharestRon.TotalPremiumInBase);

            var bucharestEur = result.Single(r => r.GroupName == "Bucharest" && r.Currency == "EUR");
            Assert.Equal(1, bucharestEur.PoliciesCount);
            Assert.Equal(150m, bucharestEur.TotalPremium);
            Assert.Equal(330m, bucharestEur.TotalPremiumInBase);

            var clujRon = result.Single(r => r.GroupName == "Cluj" && r.Currency == "RON");
            Assert.Equal(1, clujRon.PoliciesCount);
            Assert.Equal(50m, clujRon.TotalPremium);
            Assert.Equal(50m, clujRon.TotalPremiumInBase);
        }

        [Fact]
        public void ApplyFilters_AppliesStatusAndDateRange()
        {
            var now = DateTime.UtcNow;
            var older = now.AddDays(-10);
            var items = new List<PolicyReportAggregate>
            {
                new() { PolicyId = Guid.NewGuid(), City = "A", Currency = "RON", FinalPremium = 100m, FinalPremiumInBase = 100m, CreatedAt = older, Status = "Active", BuildingType = "Residential" },
                new() { PolicyId = Guid.NewGuid(), City = "B", Currency = "RON", FinalPremium = 200m, FinalPremiumInBase = 200m, CreatedAt = now, Status = "Cancelled", BuildingType = "Residential" },
                new() { PolicyId = Guid.NewGuid(), City = "C", Currency = "RON", FinalPremium = 300m, FinalPremiumInBase = 300m, CreatedAt = now, Status = "Active", BuildingType = "Commercial" },
            }.AsQueryable();

            var from = now.AddDays(-5);
            var to = now.AddDays(1);
            var dto = new GetPoliciesReportRequestDto(from, to, PolicyStatus.Active, null, null);
            var request = new GetPoliciesReportQuery(dto, ReportGroupingType.City);

            var filtered = items.ApplyFilters(request).ToList();

            Assert.Single(filtered);
            Assert.Equal("C", filtered[0].City);
            Assert.Equal("Active", filtered[0].Status);
            Assert.InRange(filtered[0].CreatedAt, from, to);
        }

        [Fact]
        public void BrokerReportGrouping_GroupsAndSumsCorrectly()
        {
            var now = DateTime.UtcNow;
            var items = new List<PolicyReportAggregate>
            {
                new() { PolicyId = Guid.NewGuid(), BrokerCode = "BR1", Currency = "RON", FinalPremium = 100m, FinalPremiumInBase = 100m, CreatedAt = now, Status = "Active", BuildingType = "Residential" },
                new() { PolicyId = Guid.NewGuid(), BrokerCode = "BR1", Currency = "RON", FinalPremium = 50m, FinalPremiumInBase = 50m, CreatedAt = now, Status = "Active", BuildingType = "Residential" },
                new() { PolicyId = Guid.NewGuid(), BrokerCode = "BR2", Currency = "EUR", FinalPremium = 200m, FinalPremiumInBase = 440m, CreatedAt = now, Status = "Active", BuildingType = "Commercial" }
            }.AsQueryable();

            var strategy = new BrokerReportGrouping();

            var result = strategy.Group(items).ToList();

            Assert.Equal(2, result.Count);

            var br1 = result.Single(r => r.GroupName == "BR1" && r.Currency == "RON");
            Assert.Equal(2, br1.PoliciesCount);
            Assert.Equal(150m, br1.TotalPremium);
            Assert.Equal(150m, br1.TotalPremiumInBase);

            var br2 = result.Single(r => r.GroupName == "BR2" && r.Currency == "EUR");
            Assert.Equal(1, br2.PoliciesCount);
            Assert.Equal(200m, br2.TotalPremium);
            Assert.Equal(440m, br2.TotalPremiumInBase);
        }

        [Fact]
        public void CountryReportGrouping_GroupsAndSumsCorrectly()
        {
            var now = DateTime.UtcNow;
            var items = new List<PolicyReportAggregate>
            {
                new() { PolicyId = Guid.NewGuid(), Country = "RO", Currency = "RON", FinalPremium = 120m, FinalPremiumInBase = 120m, CreatedAt = now, Status = "Active", BuildingType = "Residential" },
                new() { PolicyId = Guid.NewGuid(), Country = "RO", Currency = "EUR", FinalPremium = 80m, FinalPremiumInBase = 176m, CreatedAt = now, Status = "Active", BuildingType = "Residential" },
                new() { PolicyId = Guid.NewGuid(), Country = "BG", Currency = "RON", FinalPremium = 50m, FinalPremiumInBase = 50m, CreatedAt = now, Status = "Active", BuildingType = "Commercial" }
            }.AsQueryable();

            var strategy = new CountryReportGrouping();

            var result = strategy.Group(items).ToList();

            Assert.Equal(3, result.Count); 

            var roRon = result.Single(r => r.GroupName == "RO" && r.Currency == "RON");
            Assert.Equal(1, roRon.PoliciesCount);
            Assert.Equal(120m, roRon.TotalPremium);

            var roEur = result.Single(r => r.GroupName == "RO" && r.Currency == "EUR");
            Assert.Equal(1, roEur.PoliciesCount);
            Assert.Equal(80m, roEur.TotalPremium);

            var bgRon = result.Single(r => r.GroupName == "BG" && r.Currency == "RON");
            Assert.Equal(1, bgRon.PoliciesCount);
            Assert.Equal(50m, bgRon.TotalPremium);
        }

        [Fact]
        public void ApplyFilters_FiltersByCurrencyAndBuildingType()
        {
            var now = DateTime.UtcNow;
            var items = new List<PolicyReportAggregate>
            {
                new() { PolicyId = Guid.NewGuid(), City = "A", Currency = "RON", FinalPremium = 100m, FinalPremiumInBase = 100m, CreatedAt = now, Status = "Active", BuildingType = "Residential" },
                new() { PolicyId = Guid.NewGuid(), City = "B", Currency = "EUR", FinalPremium = 200m, FinalPremiumInBase = 440m, CreatedAt = now, Status = "Active", BuildingType = "Industrial" },
                new() { PolicyId = Guid.NewGuid(), City = "C", Currency = "RON", FinalPremium = 300m, FinalPremiumInBase = 300m, CreatedAt = now, Status = "Active", BuildingType = "Industrial" },
            }.AsQueryable();

            var from = now.AddMinutes(-5);
            var to = now.AddMinutes(5);

            var dto = new GetPoliciesReportRequestDto(from, to, null, "RON", BuildingType.Industrial);
            var request = new GetPoliciesReportQuery(dto, ReportGroupingType.City);

            var filtered = items.ApplyFilters(request).ToList();

            Assert.Single(filtered);
            Assert.Equal("C", filtered[0].City);
            Assert.Equal("RON", filtered[0].Currency);
            Assert.Equal("Industrial", filtered[0].BuildingType);
        }
    }
}

