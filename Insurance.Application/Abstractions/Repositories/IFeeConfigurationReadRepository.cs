using Insurance.Application.Common.Paging;
using Insurance.Application.Metadata.FeeConfiguration.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Abstractions.Repositories
{
    public interface IFeeConfigurationReadRepository
    {
        Task<PagedResult<FeeConfigurationDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    }
}
