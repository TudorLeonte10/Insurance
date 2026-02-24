using Insurance.Domain.Buildings;
using Insurance.Domain.Policies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.DTOs
{
    public record GetPoliciesReportRequestDto(
    DateTime From,
    DateTime To,
    PolicyStatus? Status,
    string? Currency,
    BuildingType? BuildingType
);
}
