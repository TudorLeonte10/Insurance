using Insurance.Application.Clients.DTOs;
using Insurance.Application.Common.Paging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Clients.Queries
{
    public record GetClientsQuery(int pageNumber = 1, int pageSize = 10) : IRequest<PagedResult<ClientDetailsDto>>;
}
