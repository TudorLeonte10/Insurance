using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Abstractions.Repositories
{
    public interface IPolicyReportRepository
    {
        Task<IReadOnlyList<PolicyReportDto>> GetPolicyReportAsync(GetPoliciesReportQuery request, CancellationToken ct);
    }
}
