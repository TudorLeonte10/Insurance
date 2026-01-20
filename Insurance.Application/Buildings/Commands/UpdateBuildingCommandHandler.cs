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
        private readonly IMapper _mapper;

        public UpdateBuildingCommandHandler(IBuildingRepository repository, IUnitOfWork uow, IMapper mapper)
        {
            _repository = repository;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(UpdateBuildingCommand request, CancellationToken ct)
        {
            var building = await _repository.GetBuildingByIdAsync(request.BuildingId, ct);

            if (building is null)
                throw new NotFoundException($"Building not found");

            _mapper.Map(request.BuildingDto, building);

            building.RiskIndicators = request.BuildingDto.RiskIndicators.Distinct()
            .Select(r => new BuildingRiskIndicator
            {
                RiskIndicator = r
            })
            .ToList();

            await _uow.SaveChangesAsync(ct);
            return building.Id;
        }
    }
}
