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
    public record GetPoliciesReportQuery(GetPoliciesReportRequestDto Dto, ReportGroupingType GroupingType) : IRequest<IEnumerable<PolicyReportDto>>;

}
