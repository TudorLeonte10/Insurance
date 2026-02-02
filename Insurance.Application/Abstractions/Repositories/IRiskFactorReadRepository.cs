using Insurance.Application.Common.Paging;
using Insurance.Application.Metadata.RiskFactors.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Abstractions.Repositories
{
    public interface IRiskFactorReadRepository
    {
        Task<PagedResult<RiskFactorConfigurationDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct);
    }
}
