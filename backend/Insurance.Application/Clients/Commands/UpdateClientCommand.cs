using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Insurance.Application.Clients.DTOs;
using Insurance.Application.Common;

namespace Insurance.Application.Clients.Commands
{
    public record UpdateClientCommand(Guid ClientId, UpdateClientDto Dto) : IRequest<Guid>, IRequireBrokerValidation;
}
