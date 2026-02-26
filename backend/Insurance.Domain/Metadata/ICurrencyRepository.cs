using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Domain.Metadata
{
    public interface ICurrencyRepository
    {
            Task AddAsync(Currency currency, CancellationToken cancellationToken);
            Task<Currency?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
            Task UpdateAsync(Currency currency, CancellationToken cancellationToken);
    }
}
