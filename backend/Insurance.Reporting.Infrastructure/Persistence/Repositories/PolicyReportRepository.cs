using AutoMapper.QueryableExtensions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Exceptions;
using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Queries;
using Insurance.Domain.Buildings;
using Insurance.Domain.Policies;
using Insurance.Infrastructure.Reports;
using Insurance.Reporting.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Reporting.Infrastructure.Persistence.Repositories
{
    public class PolicyReportRepository : IPolicyReportRepository
    {
        private readonly ReportingDbContext _dbContext;
        private readonly IEnumerable<IPolicyReportGrouping> _groupings;

        public PolicyReportRepository(ReportingDbContext dbContext, IEnumerable<IPolicyReportGrouping> groupings)
        {
            _dbContext = dbContext;
            _groupings = groupings;
        }

        public async Task<IEnumerable<PolicyByCityDto>> GetPoliciesByCityReportAsync(CancellationToken ct)
        {
            return await _dbContext.PolicyReportAggregates
                .AsNoTracking()
                .GroupBy(p => p.City)
                .Select(g => new PolicyByCityDto
                {
                    City = g.Key,
                    PolicyCount = g.Count()
                })
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<PolicyReportDto>> GetPolicyReportAsync(GetPoliciesReportQuery request, CancellationToken ct)
        {
            var query = _dbContext.PolicyReportAggregates
                .AsNoTracking()
                .AsQueryable();

            var filteredQuery = query.ApplyFilters(request);

            var grouping = _groupings
                .FirstOrDefault(x => x.GroupingType == request.GroupingType);

            if (grouping is null)
                throw new NotFoundException("Grouping not found.");

            var groupedQuery = grouping.Group(filteredQuery);

            return await groupedQuery.ToListAsync(ct);
        }

        public async Task<PolicySummaryDto> GetPolicySummaryAsync(DateTime? from, DateTime? to, string? status, string? currency, string? buildingType, CancellationToken ct)
        {
            var query = _dbContext.PolicyReportAggregates.AsNoTracking().AsQueryable();

            if (from.HasValue && to.HasValue)
                query = query.FilterByDateRange(from.Value, to.Value);

            if (!string.IsNullOrEmpty(status) && Enum.TryParse<PolicyStatus>(status, true, out var parsedStatus))
                query = query.FilterByStatus(parsedStatus);

            if (!string.IsNullOrEmpty(currency))
                query = query.FilterByCurrency(currency);

            if (!string.IsNullOrEmpty(buildingType) && Enum.TryParse<BuildingType>(buildingType, true, out var parsedBuildingType))
                query = query.FilterByBuildingType(parsedBuildingType);

            return await query
                .GroupBy(_ => 1)
                .Select(g => new PolicySummaryDto
                {
                    TotalPolicies = g.Count(),
                    TotalPremium = g.Where(p => p.Status != PolicyStatus.UnderReview.ToString() && p.Status != PolicyStatus.Rejected.ToString()).Sum(p => p.FinalPremiumInBase),
                    AveragePremium = g.Average(p => p.FinalPremiumInBase),
                    ActivePolicies = g.Count(p => p.Status == "Active"),
                    UnderReviewPolicies = g.Count(p => p.Status == "UnderReview")
                })
                .FirstOrDefaultAsync(ct) ?? new PolicySummaryDto();
        }

        public async Task<IEnumerable<PolicyTimeseriesDto>> GetPolicyTimeseriesAsync(DateTime? from, DateTime? to, string? status, string? currency, string? buildingType, CancellationToken ct)
        {
            var query = _dbContext.PolicyReportAggregates.AsNoTracking().AsQueryable();

            if (from.HasValue && to.HasValue)
                query = query.FilterByDateRange(from.Value, to.Value);

            if (!string.IsNullOrEmpty(status) && Enum.TryParse<PolicyStatus>(status, true, out var parsedStatus))
                query = query.FilterByStatus(parsedStatus);

            if (!string.IsNullOrEmpty(currency))
                query = query.FilterByCurrency(currency);

            if (!string.IsNullOrEmpty(buildingType) && Enum.TryParse<BuildingType>(buildingType, true, out var parsedBuildingType))
                query = query.FilterByBuildingType(parsedBuildingType);

            var raw = await query
                .GroupBy(p => new { p.StartDate.Year, p.StartDate.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    PolicyCount = g.Count(),
                    TotalPremium = g.Where(p => p.Status != PolicyStatus.UnderReview.ToString() && p.Status != PolicyStatus.Rejected.ToString()).Sum(p => p.FinalPremiumInBase)
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToListAsync(ct);

            return raw.Select(x => new PolicyTimeseriesDto
            {
                Date = new DateTime(x.Year, x.Month, 1),
                PolicyCount = x.PolicyCount,
                TotalPremium = x.TotalPremium
            });
        }
    }
}
