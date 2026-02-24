using Insurance.Application.Clients.DTOs;
using Insurance.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Clients.Commands
{
    public record CreateClientCommand(CreateClientDto Dto) : IRequest<Guid>, IRequireBrokerValidation;
}
