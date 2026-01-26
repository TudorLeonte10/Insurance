using AutoMapper;
using Insurance.Application.Abstractions;
using Insurance.Application.Abstractions.Repositories;
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

        public UpdateBuildingCommandHandler(IBuildingRepository repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }

        public async Task<Guid> Handle(UpdateBuildingCommand request, CancellationToken cancellationToken)
        {
            var building = await _repository.GetBuildingByIdAsync(request.BuildingId, cancellationToken);

            if (building is null)
                throw new NotFoundException("Building not found");

            building.Street = request.BuildingDto.Street;
            building.Number = request.BuildingDto.Number;
            building.ConstructionYear = request.BuildingDto.ConstructionYear;
            building.SurfaceArea = request.BuildingDto.SurfaceArea;
            building.NumberOfFloors = request.BuildingDto.NumberOfFloors;
            building.InsuredValue = request.BuildingDto.InsuredValue;

            await _repository.UpdateAsync(building, request.BuildingDto.RiskIndicators, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return building.Id;
        }
    }
}
