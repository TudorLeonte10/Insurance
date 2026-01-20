using AutoMapper;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Buildings.DTOs;
using Insurance.Domain.Buildings;
using Insurance.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Buildings.Queries
{
    public class GetBuildingByIdQueryHandler : IRequestHandler<GetBuildingByIdQuery, BuildingDetailsDto>
    {
        private readonly IBuildingRepository _buildingRepository;
        private readonly IMapper _mapper;

        public GetBuildingByIdQueryHandler(IBuildingRepository buildingRepository, IMapper mapper)
        {
            _buildingRepository = buildingRepository;
            _mapper = mapper;
        }

        public async Task<BuildingDetailsDto> Handle(GetBuildingByIdQuery request, CancellationToken cancellationToken)
        {
            var building = await _buildingRepository.GetBuildingByIdAsync(request.BuildingId, cancellationToken);

            if (building == null)
            {
                throw new NotFoundException($"Building { request.BuildingId } not found.");
            }

            return _mapper.Map<BuildingDetailsDto>(building);
        }
    }
}
