using Insurance.Application.Common.Paging;
using Insurance.Application.Metadata.RiskFactors.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Metadata.RiskFactors.Queries
{
    public record GetAllRiskFactorsQuery(int pageNumber, int pageSize) : IRequest<PagedResult<RiskFactorConfigurationDto>>;

}
