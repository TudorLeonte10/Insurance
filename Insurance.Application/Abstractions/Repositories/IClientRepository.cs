using Insurance.Application.Clients.DTOs;
using Insurance.Application.Common.Paging;
using Insurance.Domain.Clients;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Domain.Abstractions.Repositories
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<bool> ExistsByIdentifierAsync(string identifier, CancellationToken cancellationToken);
        Task<Client?> GetByIdentificationNumberAsync(string identifier, CancellationToken cancellationToken);

        Task<PagedResult<ClientDetailsDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<PagedResult<ClientDetailsDto>> SearchAsync(string? name, string? identifier, int pageNumber, int pageSize, CancellationToken cancellationToken);

    }
}
