using Insurance.Application.Brokers.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Brokers.Queries
{
   public record GetBrokerByIdQuery(Guid brokerId) : IRequest<BrokerDetailsDto>;
}
