using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Enums;
using Insurance.Reporting.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Reports
{
    public interface IPolicyReportGrouping
    {
        public ReportGroupingType GroupingType { get; }
        IQueryable<PolicyReportDto> Group(IQueryable<PolicyReportAggregate> data);
    }
}
