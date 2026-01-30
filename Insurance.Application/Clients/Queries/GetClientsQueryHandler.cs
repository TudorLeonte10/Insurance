using AutoMapper;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Clients.DTOs;
using Insurance.Application.Common.Paging;
using Insurance.Application.Exceptions;
using Insurance.Domain.Clients;
using Insurance.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Clients.Queries
{
    public class GetClientsQueryHandler
    : IRequestHandler<GetClientsQuery, PagedResult<ClientDetailsDto>>
    {
        private readonly IClientSearchRepository _searchRepository;
        private readonly IClientReadRepository _readRepository;

        public GetClientsQueryHandler(
            IClientSearchRepository searchRepository,
            IClientReadRepository readRepository)
        {
            _searchRepository = searchRepository;
            _readRepository = readRepository;
        }

        public async Task<PagedResult<ClientDetailsDto>> Handle(
            GetClientsQuery request,
            CancellationToken ct)
        {
            if (request.ClientId.HasValue)
            {
                var client = await _readRepository
                    .GetByIdAsync(request.ClientId.Value, ct);

                if (client is null)
                    throw new NotFoundException(
                        $"Client with ID {request.ClientId} not found.");

                return new PagedResult<ClientDetailsDto>(
                    new[] { client },
                    request.PageNumber,
                    request.PageSize,
                    1);
            }

            return await _searchRepository.SearchAsync(
                request.Name,
                request.IdentificationNumber,
                request.PageNumber,
                request.PageSize,
                ct);
        }
    }

}
