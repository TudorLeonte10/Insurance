using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Insurance.Application.Clients.DTOs;

namespace Insurance.Application.Clients.Commands
{
    public record UpdateClientCommand(
        Guid ClientId,
        UpdateClientDto Dto
    ) : IRequest<Guid>;
}
