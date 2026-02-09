using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Domain.Clients
{
    public interface IClientRepository
    {
        Task<Client> GetByIdAsync(Guid id, CancellationToken ct);
        Task AddAsync(Client client, CancellationToken ct);
        Task UpdateAsync(Client client, CancellationToken ct);
    }

}
