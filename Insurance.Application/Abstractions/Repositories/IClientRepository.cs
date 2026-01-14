using Insurance.Domain.Clients;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Domain.Abstractions.Repositories
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<bool> ExistsByIdentifierAsync(string identifier, CancellationToken cancellationToken);
        Task<Client?> GetByIdentificationNumberAsync(string identifier, CancellationToken cancellationToken);
    }
}
