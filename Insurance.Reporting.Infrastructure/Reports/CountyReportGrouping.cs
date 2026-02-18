using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Enums;
using Insurance.Reporting.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Reports
{
    public class CountyReportGrouping : IPolicyReportGrouping
    {
        public ReportGroupingType GroupingType => ReportGroupingType.County;

        public IQueryable<PolicyReportDto> Group(IQueryable<PolicyReportAggregate> data)
        {
            return data.GroupBy(x => new { x.County, x.Currency, })
                .Select(g => new PolicyReportDto
                {
                    GroupName = g.Key.County,
                    Currency = g.Key.Currency,
                    PoliciesCount = g.Count(),
                    TotalPremium = g.Sum(x => x.FinalPremium),
                    TotalPremiumInBase = g.Sum(x => x.FinalPremiumInBase)
                })
                .OrderBy(x => x.GroupName)
                .ThenBy(x => x.Currency);
        }
    }
}
