using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Policy.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Statistics.Queries
{
    public class GetPolicySummaryQueryHandler : IRequestHandler<GetPolicySummaryQuery, PolicySummaryDto>
    {
        private readonly IPolicyReportRepository _reportRepository;
        public GetPolicySummaryQueryHandler(IPolicyReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        public async Task<PolicySummaryDto> Handle(GetPolicySummaryQuery request, CancellationToken cancellationToken)
        {
            return await _reportRepository.GetPolicySummaryAsync(request.From, request.To, request.Status, request.Currency, request.BuildingType, cancellationToken);
        }
    }
}
