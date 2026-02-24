using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Queries
{
    public class GetPoliciesReportQueryHandler : IRequestHandler<GetPoliciesReportQuery, IEnumerable<PolicyReportDto>>
    {
        private readonly IPolicyReportRepository _repo;
        public GetPoliciesReportQueryHandler(IPolicyReportRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<PolicyReportDto>> Handle(GetPoliciesReportQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetPolicyReportAsync(request, cancellationToken);
        }
    }
}
