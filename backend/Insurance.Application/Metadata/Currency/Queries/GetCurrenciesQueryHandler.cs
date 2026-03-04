using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Common.Paging;
using Insurance.Application.Metadata.Currency.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Metadata.Currency.Queries
{
    public class GetCurrenciesQueryHandler : IRequestHandler<GetCurrenciesQuery, PagedResult<CurrencyDto>>
    {
        private readonly ICurrencyReadRepository _readRepo;
        public GetCurrenciesQueryHandler(ICurrencyReadRepository readRepo)
        {
            _readRepo = readRepo;
        }
        public async Task<PagedResult<CurrencyDto>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
        {
            var result = await _readRepo.GetPagedAsync(request.PageNumber, request.PageSize, cancellationToken);
            return result;
        }
    }
}
