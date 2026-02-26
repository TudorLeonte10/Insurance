using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Abstractions
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
