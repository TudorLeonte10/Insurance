using Insurance.Application.Common.Paging;
using Insurance.Application.Metadata.Currency.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Abstractions.Repositories
{
    public interface ICurrencyReadRepository
    {
        Task<PagedResult<CurrencyDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    }
}
