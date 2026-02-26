using Insurance.Application.Common.Paging;
using Insurance.Application.Metadata.FeeConfiguration.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Metadata.FeeConfiguration.Queries
{
    public record GetFeeConfigurationQuery(int pageNumber, int pageSize) : IRequest<PagedResult<FeeConfigurationDto>>;
   
}
