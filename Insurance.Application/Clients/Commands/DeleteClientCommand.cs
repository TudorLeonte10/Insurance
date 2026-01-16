using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Clients.Commands
{
    public record DeleteClientCommand(Guid ClientId) : IRequest<Guid>;
}
