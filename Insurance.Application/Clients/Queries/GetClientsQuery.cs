using Insurance.Application.Clients.DTOs;
using Insurance.Application.Common;
using Insurance.Application.Common.Paging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Clients.Queries
{
    public class GetClientsQuery : IRequest<PagedResult<ClientDetailsDto>>, IRequireBrokerValidation
    {
        public Guid? ClientId { get; init; }

        public string? Name { get; init; }
        public string? IdentificationNumber { get; init; }

        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }

}
