using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Enums;
using Insurance.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Reports
{
    public class CityReportGrouping : IPolicyReportGrouping
    {
        public ReportGroupingType GroupingType => ReportGroupingType.City;

        public IQueryable<PolicyReportDto> Group(IQueryable<PolicyEntity> data)
        {
            return data.GroupBy(x => new {x.Building.City.Id, x.Building.City.Name, x.Currency.Code, x.Currency.ExchangeRateToBase })
                .Select(g => new PolicyReportDto
                {
                    GroupName = g.Key.Name,
                    Currency = g.Key.Code,
                    PoliciesCount = g.Count(),
                    TotalPremium = g.Sum(x => x.FinalPremium),
                    TotalPremiumInBase = g.Sum(x => x.FinalPremium) * g.Key.ExchangeRateToBase
                })
                .OrderBy(x => x.GroupName)
                .ThenBy(x => x.Currency);
        }
    }
}
