using AutoMapper;
using Insurance.Application.Clients.DTOs;
using Insurance.Domain.Abstractions.Repositories;
using Insurance.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Clients.Queries
{
    public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, ClientDetailsDto>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public GetClientByIdQueryHandler(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<ClientDetailsDto> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetByIdAsync(request.ClientId, cancellationToken);

            if (client == null)
            {
                throw new NotFoundException($"Client with ID {request.ClientId} not found.");
            }

            return _mapper.Map<ClientDetailsDto>(client);
        }
    }
}
