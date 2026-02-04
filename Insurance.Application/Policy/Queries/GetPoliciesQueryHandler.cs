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
    public class GetPoliciesQueryHandler
    : IRequestHandler<GetPoliciesQuery, PagedResult<PolicyDetailsDto>>
    {
        private readonly IPolicyReadRepository _readRepository;
        private readonly IPolicySearchRepository _searchRepository;

        public GetPoliciesQueryHandler(
            IPolicyReadRepository readRepository,
            IPolicySearchRepository searchRepository)
        {
            _readRepository = readRepository;
            _searchRepository = searchRepository;
        }

        public async Task<PagedResult<PolicyDetailsDto>> Handle(
            GetPoliciesQuery request,
            CancellationToken ct)
        {
            if (request.PolicyId.HasValue)
            {
                var policy = await _readRepository
                    .GetByIdAsync(request.PolicyId.Value, ct);

                if (policy is null)
                    throw new NotFoundException(
                        $"Policy with ID {request.PolicyId} not found.");

                return new PagedResult<PolicyDetailsDto>(
                    new[] { policy },
                    request.PageNumber,
                    request.PageSize,
                    1);
            }

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
