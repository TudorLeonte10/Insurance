using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Brokers.Commands
{
    public record ChangeBrokerStatusCommand(Guid BrokerId, Guid UserId, bool IsActive) : IRequest<Guid>;

}
