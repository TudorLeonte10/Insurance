using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Brokers.Commands
{
    public record ChangeBrokerStatusCommand(Guid BrokerId, bool IsActive) : IRequest<Guid>;

}
