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
    }
}
