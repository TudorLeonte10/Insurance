using AutoMapper;
using Insurance.Application.Abstractions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Authentication;
using Insurance.Application.Exceptions;
using Insurance.Domain.Buildings;
using Insurance.Domain.Clients;
using Insurance.Domain.Exceptions;
using Insurance.Domain.RiskIndicators;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Buildings.Commands
{
    public class UpdateBuildingCommandHandler : IRequestHandler<UpdateBuildingCommand, Guid>
    {
        private readonly IBuildingRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IClientRepository _clientRepository;

        public UpdateBuildingCommandHandler(IBuildingRepository repository, IUnitOfWork uow, ICurrentUserContext currentUserContext, IClientRepository clientRepository)
        {
            _repository = repository;
            _uow = uow;
            _currentUserContext = currentUserContext;
            _clientRepository = clientRepository;
        }

        public async Task<Guid> Handle(UpdateBuildingCommand request, CancellationToken cancellationToken)
        {
            var building = await ValidateAndGetBuildingAsync(request, cancellationToken);

            building.UpdateDetails(
                request.BuildingDto.ConstructionYear,
                request.BuildingDto.NumberOfFloors,
                request.BuildingDto.SurfaceArea,
                request.BuildingDto.InsuredValue,
                request.BuildingDto.Street,
                request.BuildingDto.Number);

            await _repository.UpdateAsync(building, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return building.Id;
        }

        private async Task<Building> ValidateAndGetBuildingAsync(UpdateBuildingCommand request, CancellationToken ct)
        {
            var brokerId = _currentUserContext.BrokerId;
               

            var building = await _repository.GetByIdAsync(request.BuildingId, ct);
            if (building is null)
                throw new NotFoundException($"Building with id {request.BuildingId} not found");

            var client = await _clientRepository.GetByIdAsync(building.ClientId, ct);
            if (client is null || client.BrokerId != brokerId)
                throw new ForbiddenException($"Building with id {request.BuildingId} does not belong to the current broker");

            return building;
        }
    }
}