using AutoMapper;
using Insurance.Application.Abstractions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Domain.Abstractions.Repositories;
using Insurance.Domain.Buildings;
using Insurance.Domain.Exceptions;
using Insurance.Domain.RiskIndicators;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Buildings.Commands
{
    public class CreateBuildingCommandHandler : IRequestHandler<CreateBuildingCommand, Guid>
    {
        private readonly IBuildingRepository _buildingRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IGeographyRepository _geographyRepository;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateBuildingCommandHandler(IBuildingRepository buildingRepository, IClientRepository clientRepository, IGeographyRepository geographyRepositor, IUnitOfWork uow, IMapper mapper)
        {
            _buildingRepository = buildingRepository;
            _clientRepository = clientRepository;
            _geographyRepository = geographyRepositor;
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<Guid> Handle(CreateBuildingCommand request, CancellationToken cancellationToken)
        {
            await ValidateCreateBuildingAsync(request, cancellationToken);

            var building = CreateBuilding(request);

            await _buildingRepository.AddBuildingAsync(building, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return building.Id;
        }


        private async Task ValidateCreateBuildingAsync(CreateBuildingCommand request, CancellationToken cancellationToken)
        {
            if (!await _clientRepository.ExistsAsync(request.ClientId, cancellationToken))
            {
                throw new NotFoundException(
                    $"Client with Id {request.ClientId} was not found.");
            }

            if (!await _geographyRepository.ExistsCityAsync(
                    request.BuildingDto.CityId,
                    cancellationToken))
            {
                throw new NotFoundException(
                    $"City with Id {request.BuildingDto.CityId} was not found.");
            }
        }

        private Building CreateBuilding(CreateBuildingCommand request)
        {
            var building = _mapper.Map<Building>(request.BuildingDto);

            building.ClientId = request.ClientId;

            building.RiskIndicators = request.BuildingDto.RiskIndicators
                .Distinct()
                .Select(r => new BuildingRiskIndicator
                {
                    RiskIndicator = r
                })
                .ToList();

            return building;
        }

    }
}
