using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Common.Paging;
using Insurance.Application.Metadata.RiskFactors.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Metadata.RiskFactors.Queries
{
    public class GetAllRiskFactorsQueryHandler : IRequestHandler<GetAllRiskFactorsQuery, PagedResult<RiskFactorConfigurationDto>>
    {
        private readonly IRiskFactorReadRepository _riskFactorReadRepository;
        public GetAllRiskFactorsQueryHandler(IRiskFactorReadRepository riskFactorReadRepository)
        {
            _riskFactorReadRepository = riskFactorReadRepository;
        }
        public async Task<PagedResult<RiskFactorConfigurationDto>> Handle(GetAllRiskFactorsQuery request, CancellationToken cancellationToken)
        {
            var riskFactors = await _riskFactorReadRepository.GetPagedAsync(request.pageNumber, request.pageSize, cancellationToken);
            return riskFactors;
        }
    }
}
