using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Authentication;
using Insurance.Application.Buildings.DTOs;
using Insurance.Application.Exceptions;
using Insurance.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Buildings.Queries
{
    public class GetBuildingsQueryHandler
    : IRequestHandler<GetBuildingsQuery, IReadOnlyList<BuildingDetailsDto>>
    {
        private readonly IBuildingReadRepository _repository;
        private readonly IClientReadRepository _clientRepository;
        private readonly ICurrentUserContext _currentUserContext;

        public GetBuildingsQueryHandler(
            IBuildingReadRepository repository,
            IClientReadRepository clientRepository,
            ICurrentUserContext currentUserContext)
        {
            _repository = repository;
            _clientRepository = clientRepository;
            _currentUserContext = currentUserContext;
        }

        public async Task<IReadOnlyList<BuildingDetailsDto>> Handle(
            GetBuildingsQuery request,
            CancellationToken cancellationToken)
        {
            var brokerId = _currentUserContext.BrokerId!.Value;
            
            if (request.BuildingId.HasValue)
            {
                var building = await _repository.GetByIdAsync(
                    request.BuildingId.Value,
                    cancellationToken);

                if (building is null)
                    throw new NotFoundException(
                        $"Building {request.BuildingId} not found.");

                await ValidateBuildingOwnershipAsync(building.ClientId, brokerId, cancellationToken);

                return new[] { building };
            }

            if (request.ClientId.HasValue)
            {
                await ValidateClientOwnershipAsync(request.ClientId.Value, brokerId, cancellationToken);
                return await _repository.GetByClientIdAsync(
                    request.ClientId.Value,
                    cancellationToken);
            }

            throw new NotFoundException(
                "Either BuildingId or ClientId must be provided.");
        }

        private async Task ValidateBuildingOwnershipAsync(Guid clientId, Guid brokerId, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetByIdAsync(clientId, cancellationToken);
            if (client is null)
                throw new NotFoundException($"Client {clientId} not found.");

            if (client.BrokerId != brokerId)
                throw new ForbiddenException("You do not have access to this client's buildings.");
        }

        private async Task ValidateClientOwnershipAsync(
            Guid clientId,
            Guid brokerId,
            CancellationToken ct)
        {
            var client = await _clientRepository.GetByIdAsync(clientId, ct);

            if (client is null)
                throw new NotFoundException($"Client {clientId} not found.");

            if (client.BrokerId != brokerId)
                throw new ForbiddenException($"Client {clientId} does not belong to the current broker.");
        }
    }
}

