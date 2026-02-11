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
        public IEnumerable<PolicyReportDto> Group(IEnumerable<PolicyReportReadModel> data)
        {
            return data.GroupBy(x => new { x.Country, x.CurrencyCode })
                .Select(g => new PolicyReportDto(
                    g.Key.Country,
                    g.Key.CurrencyCode,
                    g.Count(),
                    g.Sum(x => x.FinalPremium),
                    g.Sum(x => x.FinalPremium * x.ExchangeRate)
                ))
                .OrderBy(x => x.GroupName)
                .ThenBy(x => x.Currency);
        }
    }
}
