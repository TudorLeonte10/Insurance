using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Services.Strategies
{
    public interface IReportGroupingStrategy
    {
        ReportGroupingType GroupingType { get; }
        IQueryable<PolicyReportDto> Group(IQueryable<PolicyReportReadModel> data);
    }
}
