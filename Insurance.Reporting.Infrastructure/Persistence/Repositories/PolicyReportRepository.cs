using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Exceptions;
using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Queries;
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
    }
}
