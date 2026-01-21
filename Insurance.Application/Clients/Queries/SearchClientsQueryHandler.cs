using AutoMapper;
using Insurance.Application.Clients.DTOs;
using Insurance.Application.Common.Paging;
using Insurance.Domain.Abstractions.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Clients.Queries
{
    public class SearchClientsQueryHandler : IRequestHandler<SearchClientsQuery, PagedResult<ClientDetailsDto>>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;
        public SearchClientsQueryHandler(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }
        public async Task<PagedResult<ClientDetailsDto>> Handle(SearchClientsQuery request, CancellationToken cancellationToken)
        {
            return await _clientRepository.SearchAsync(
                request.Name, 
                request.IdentificationNumber, 
                request.PageNumber, 
                request.PageSize, 
                cancellationToken);

        }
    }
}
