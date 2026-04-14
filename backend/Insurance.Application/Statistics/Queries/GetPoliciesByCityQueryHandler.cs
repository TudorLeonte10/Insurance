using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Policy.DTOs;
using Insurance.Domain.Policies;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Statistics.Queries
{
    public class GetPoliciesByCityQueryHandler : IRequestHandler<GetPoliciesByCityReportQuery, IEnumerable<PolicyByCityDto>>
    {
        private readonly IPolicyReportRepository _reportRepository;
        public GetPoliciesByCityQueryHandler(IPolicyReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        public async Task<IEnumerable<PolicyByCityDto>> Handle(GetPoliciesByCityReportQuery request, CancellationToken cancellationToken)
        {
            return await _reportRepository.GetPoliciesByCityReportAsync(cancellationToken);
        }
    }
}
