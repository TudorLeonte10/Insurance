using Insurance.Application.Brokers.DTOs;
using Insurance.Application.Common.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Abstractions.Repositories
{
    public interface IBrokerReadRepository
    {
        Task<BrokerDetailsDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<PagedResult<BrokerDetailsDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

    }
}
