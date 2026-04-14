using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Abstractions.Repositories
{
    public interface IPolicyReportRepository
    {
        Task<IEnumerable<PolicyReportDto>> GetPolicyReportAsync(GetPoliciesReportQuery request, CancellationToken ct);
        Task<IEnumerable<PolicyByCityDto>> GetPoliciesByCityReportAsync(CancellationToken ct);
        Task<PolicySummaryDto> GetPolicySummaryAsync(DateTime? from, DateTime? to, string? status, string? currency, string? buildingType, CancellationToken ct);
        Task<IEnumerable<PolicyTimeseriesDto>> GetPolicyTimeseriesAsync(DateTime? from, DateTime? to, string? status, string? currency, string? buildingType, CancellationToken ct);
    }
}
