using Insurance.Application.Brokers.DTOs;
using Insurance.Application.Common.Paging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Brokers.Queries
{
    public record GetBrokersQuery(int PageNumber, int PageSize) : IRequest<PagedResult<BrokerDetailsDto>>;

}
