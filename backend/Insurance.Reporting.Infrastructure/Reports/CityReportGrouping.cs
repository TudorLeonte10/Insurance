using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Enums;
using Insurance.Reporting.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Reports
{
    public class CityReportGrouping : IPolicyReportGrouping
    {
        public ReportGroupingType GroupingType => ReportGroupingType.City;

        public IQueryable<PolicyReportDto> Group(IQueryable<PolicyReportAggregate> data)
        {
            return data.GroupBy(x => new {x.City, x.Currency })
                .Select(g => new PolicyReportDto
                {
                    GroupName = g.Key.City,
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
