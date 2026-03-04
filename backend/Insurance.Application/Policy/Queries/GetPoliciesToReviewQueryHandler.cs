using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Common.Paging;
using Insurance.Application.Policy.DTOs;
using Insurance.Domain.Policies;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Queries
{
    public class GetPoliciesToReviewQueryHandler : IRequestHandler<GetPoliciesToReviewQuery, PagedResult<PolicyDetailsDto>>
    {
        private readonly IPolicyReadRepository _policyRepo;

        public GetPoliciesToReviewQueryHandler(IPolicyReadRepository policyRepo)
        {
            _policyRepo = policyRepo;
        }
        public async Task<PagedResult<PolicyDetailsDto>> Handle(GetPoliciesToReviewQuery request, CancellationToken cancellationToken)
        {
            return await _policyRepo.GetPoliciesToReviewAsync(request.PageNumber, request.PageSize, cancellationToken);
        }
    }
}
