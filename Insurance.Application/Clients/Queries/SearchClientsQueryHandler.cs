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
        public SearchClientsQueryHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }
        public async Task<PagedResult<ClientDetailsDto>> Handle(SearchClientsQuery request, CancellationToken cancellationToken)
        {
            return await _clientRepository.SearchAsync(
                request.name, 
                request.identificationNumber, 
                request.pageNumber, 
                request.pageSize, 
                cancellationToken);

        }
    }
}
