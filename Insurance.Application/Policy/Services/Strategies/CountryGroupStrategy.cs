using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Services.Strategies
{
    public class CountryGroupStrategy : IReportGroupingStrategy
    {
        public ReportGroupingType GroupingType => ReportGroupingType.Country;
        public IQueryable<PolicyReportDto> Group(IQueryable<PolicyReportReadModel> data)
        {
            return data.GroupBy(x => new { x.Country, x.CurrencyCode })
                .Select(g => new PolicyReportDto
                {
                    GroupName = g.Key.Country,
                    Currency = g.Key.CurrencyCode,
                    PoliciesCount = g.Count(),
                    TotalPremium = g.Sum(x => x.FinalPremium),
                    TotalPremiumnBase = g.Sum(x => x.FinalPremiumBase)
                })
                .OrderBy(x => x.GroupName)
                .ThenBy(x => x.Currency);
        }
    }
}
