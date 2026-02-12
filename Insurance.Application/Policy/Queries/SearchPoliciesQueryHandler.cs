using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Authentication;
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
        private readonly ICurrentUserContext _currentUserContext;

        public SearchPoliciesQueryHandler(IPolicySearchRepository searchRepository, ICurrentUserContext currentUserContext)
        {
            _searchRepository = searchRepository;
            _currentUserContext = currentUserContext;
        }

        public async Task<PagedResult<PolicyDetailsDto>> Handle(
            SearchPoliciesQuery request,
            CancellationToken ct)
        {
            var brokerId = _currentUserContext.BrokerId;

            return await _searchRepository.SearchAsync(
                request.ClientId,
                brokerId,
                request.Status,
                request.StartDateFrom,
                request.StartDateTo,
                request.PageNumber,
                request.PageSize,
                ct);
        }
    }

}
