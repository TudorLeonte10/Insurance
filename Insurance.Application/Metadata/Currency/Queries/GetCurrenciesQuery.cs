using Insurance.Application.Common.Paging;
using Insurance.Application.Metadata.Currency.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Metadata.Currency.Queries
{
    public record GetCurrenciesQuery(int PageNumber, int PageSize) : IRequest<PagedResult<CurrencyDto>>;
}
