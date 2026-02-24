using Insurance.Application.Clients.DTOs;
using Insurance.Application.Common.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Abstractions.Repositories
{
    public interface IClientSearchRepository
    {
        Task<PagedResult<ClientDetailsDto>> SearchAsync(
            Guid brokerId,
            string? name,
            string? identifier,
            int pageNumber,
            int pageSize,
            CancellationToken ct);
    }

}
