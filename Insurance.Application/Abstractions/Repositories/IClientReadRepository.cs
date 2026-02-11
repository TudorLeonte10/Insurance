using Insurance.Application.Clients.DTOs;
using Insurance.Application.Common.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Abstractions.Repositories
{
    public interface IClientReadRepository
    {
        Task<ClientDetailsDto?> GetByIdAsync(
            Guid clientId,
            CancellationToken ct);

    }
}
