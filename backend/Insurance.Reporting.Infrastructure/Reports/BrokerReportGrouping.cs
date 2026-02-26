using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Enums;
using Insurance.Reporting.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Reports
{
    public class BrokerReportGrouping : IPolicyReportGrouping
    {
        public ReportGroupingType GroupingType => ReportGroupingType.Broker;

        public IQueryable<PolicyReportDto> Group(IQueryable<PolicyReportAggregate> data)
        {
            return data.GroupBy(x => new {x.BrokerCode, x.Currency })
                .Select(g => new PolicyReportDto
                {
                    GroupName = g.Key.BrokerCode,
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
