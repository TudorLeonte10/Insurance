using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Enums;
using Insurance.Domain.Buildings;
using Insurance.Domain.Policies;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Queries
{
    public record GetPoliciesReportQuery(
    ReportGroupingType GroupingType,
    DateTime? From,
    DateTime? To,
    PolicyStatus? Status,
    string? Currency,
    BuildingType? BuildingType
    ) : IRequest<IEnumerable<PolicyReportDto>>;


}
