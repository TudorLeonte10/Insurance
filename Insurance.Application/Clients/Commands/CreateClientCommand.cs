using Insurance.Application.Clients.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Clients.Commands
{
    public record CreateClientCommand(CreateClientDto Dto) : IRequest<Guid>;
}
