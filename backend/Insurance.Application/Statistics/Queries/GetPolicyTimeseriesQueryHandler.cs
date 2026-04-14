using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Policy.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Statistics.Queries
{
    public class GetPolicyTimeseriesQueryHandler : IRequestHandler<GetPolicyTimeseriesQuery, IEnumerable<PolicyTimeseriesDto>>
    {
        private readonly IPolicyReportRepository _reportRepository;
        public GetPolicyTimeseriesQueryHandler(IPolicyReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<IEnumerable<PolicyTimeseriesDto>> Handle(GetPolicyTimeseriesQuery request, CancellationToken cancellationToken)
        {
            return await _reportRepository.GetPolicyTimeseriesAsync(request.From, request.To, request.Status, request.Currency, request.BuildingType, cancellationToken);
        }
    }
}