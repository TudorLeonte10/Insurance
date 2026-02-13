using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Services;
using Insurance.Application.Policy.Services.Strategies;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Queries
{
    public class GetPoliciesReportQueryHandler : IRequestHandler<GetPoliciesReportQuery, IEnumerable<PolicyReportDto>>
    {
        private readonly IPolicyReadRepository _repo;
        private readonly IGroupingStrategyFactory _factory;
        public GetPoliciesReportQueryHandler(IPolicyReadRepository repo, IGroupingStrategyFactory factory)
        {
            _repo = repo;
            _factory = factory;
        }
        public async Task<IEnumerable<PolicyReportDto>> Handle(GetPoliciesReportQuery request, CancellationToken cancellationToken)
        {
            var baseQuery = _repo.GetQueryData().ApplyFilters(request);
            var strategy = _factory.Create(request.GroupingType);
            return await strategy.Group(baseQuery).ToListAsync(cancellationToken);
        }
    }
}
