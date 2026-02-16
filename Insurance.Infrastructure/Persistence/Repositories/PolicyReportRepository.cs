using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Enums;
using Insurance.Application.Policy.Queries;
using Insurance.Infrastructure.Extensions;
using Insurance.Infrastructure.Reports;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Repositories
{
    public class PolicyReportRepository : IPolicyReportRepository
    {
        private readonly InsuranceDbContext _dbContext;
        private readonly IEnumerable<IPolicyReportGrouping> _groupings;
        public PolicyReportRepository(InsuranceDbContext dbContext, IEnumerable<IPolicyReportGrouping> groupings)
        {
            _dbContext = dbContext;
            _groupings = groupings;
        }
        public async Task<IReadOnlyList<PolicyReportDto>> GetPolicyReportAsync(GetPoliciesReportQuery request, CancellationToken ct)
        {
            var baseQuery = _dbContext.Policies.AsNoTracking().ApplyFilters(request);

            var qroupingQuery = _groupings.First(g => g.GroupingType == request.GroupingType).Group(baseQuery);

            return await qroupingQuery.ToListAsync(ct);
        }
    }
}
