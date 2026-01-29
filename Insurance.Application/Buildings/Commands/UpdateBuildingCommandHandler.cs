using AutoMapper;
using Insurance.Application.Abstractions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Domain.Buildings;
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
            var building = await _repository.GetByIdAsync(request.BuildingId, cancellationToken);

            if (building is null)
                throw new NotFoundException("Building not found");

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
    }
}
