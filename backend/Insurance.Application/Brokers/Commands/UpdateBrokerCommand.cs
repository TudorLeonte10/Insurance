using Insurance.Application.Brokers.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Brokers.Commands
{
    public record UpdateBrokerCommand(UpdateBrokerDto brokerDto, Guid brokerId) : IRequest<Guid>;
}
