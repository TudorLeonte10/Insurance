using Insurance.Application.Clients.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Clients.Queries
{
    public record GetClientByIdQuery(Guid ClientId) : IRequest<ClientDetailsDto>;


}
