using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Domain.Brokers
{
    public interface IBrokerRepository
    {
        Task<Broker?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(Broker broker, CancellationToken cancellationToken);
        Task UpdateAsync(Broker broker, CancellationToken cancellationToken);
    }
}
