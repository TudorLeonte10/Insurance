using AutoMapper;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Authentication;
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
        private readonly ICurrentUserContext _currentUser;

        public GetClientsQueryHandler(
            IClientSearchRepository searchRepository,
            IClientReadRepository readRepository,
            ICurrentUserContext currentUser)
        {
            _searchRepository = searchRepository;
            _readRepository = readRepository;
            _currentUser = currentUser;
        }

        public async Task<PagedResult<ClientDetailsDto>> Handle(
            GetClientsQuery request,
            CancellationToken ct)
        {
            var brokerId = _currentUser.BrokerId;
               
            if (request.ClientId.HasValue)
            {
                var client = await _readRepository
                    .GetByIdAsync(request.ClientId.Value, ct);

                if (client is null)
                    throw new NotFoundException(
                        $"Client with ID {request.ClientId} not found.");

                if(client.BrokerId != brokerId)
                    throw new UnauthorizedException($"Client with ID {request.ClientId} does not belong to the current user's broker.");

                return new PagedResult<ClientDetailsDto>(
                    new[] { client },
                    request.PageNumber,
                    request.PageSize,
                    1);
            }

            return await _searchRepository.SearchAsync(
                brokerId!.Value,
                request.Name,
                request.IdentificationNumber,
                request.PageNumber,
                request.PageSize,
                ct);
        }
    }

}
