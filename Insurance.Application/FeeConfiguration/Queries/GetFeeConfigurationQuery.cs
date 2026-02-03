using Insurance.Application.Common.Paging;
using Insurance.Application.FeeConfiguration.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.FeeConfiguration.Queries
{
    public record GetFeeConfigurationQuery(int pageNumber, int pageSize) : IRequest<PagedResult<FeeConfigurationDto>>;
   
}
