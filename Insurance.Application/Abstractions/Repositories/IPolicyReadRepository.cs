using Insurance.Application.Policy.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Abstractions.Repositories
{
    public interface IPolicyReadRepository
    {
        Task<PolicyDetailsDto?> GetByIdAsync(Guid policyId, CancellationToken ct);

        IQueryable<PolicyReportReadModel> GetQueryData();
    }

}
