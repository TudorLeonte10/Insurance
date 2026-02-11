using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Brokers.DTOs;
using Insurance.Application.Exceptions;
using Insurance.Domain.Brokers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Brokers.Queries
{
    public class GetBrokerByIdQueryHandler : IRequestHandler<GetBrokerByIdQuery, BrokerDetailsDto>
    {
        private readonly IBrokerReadRepository _brokerRepository;
        public GetBrokerByIdQueryHandler(IBrokerReadRepository brokerRepository)
        {
            _brokerRepository = brokerRepository;
        }
        public async Task<BrokerDetailsDto> Handle(GetBrokerByIdQuery request, CancellationToken cancellationToken)
        {
            var broker = await _brokerRepository.GetByIdAsync(request.brokerId, cancellationToken);

            if(broker == null)
            {
                throw new NotFoundException($"Broker with ID {request.brokerId} not found.");
            }

            return broker;
        }
    }
}
