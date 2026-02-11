using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Common.Paging;
using Insurance.Application.Exceptions;
using Insurance.Application.Policy.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Queries
{
    public class SearchPoliciesQueryHandler
    : IRequestHandler<SearchPoliciesQuery, PagedResult<PolicyDetailsDto>>
    {
        private readonly IPolicySearchRepository _searchRepository;

        public SearchPoliciesQueryHandler(IPolicySearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        public async Task<PagedResult<PolicyDetailsDto>> Handle(
            SearchPoliciesQuery request,
            CancellationToken ct)
        {
            return await _searchRepository.SearchAsync(
                request.ClientId,
                request.BrokerId,
                request.Status,
                request.StartDateFrom,
                request.StartDateTo,
                request.PageNumber,
                request.PageSize,
                ct);
        }
    }

}
