using AutoMapper;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Buildings.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Buildings.Queries
{
    public class GetBuildingsByClientQueryHandler : IRequestHandler<GetBuildingsByClientQuery, IReadOnlyList<BuildingDetailsDto>>
    {
        private readonly IBuildingRepository _buildingRepository;
        private readonly IMapper _mapper;
        public GetBuildingsByClientQueryHandler(IBuildingRepository buildingRepository, IMapper mapper)
        {
            _buildingRepository = buildingRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<BuildingDetailsDto>> Handle(GetBuildingsByClientQuery request, CancellationToken cancellationToken)
        {
            var buildings = await _buildingRepository.GetAllBuildingsByClientId(request.ClientId, cancellationToken);
            return _mapper.Map<IReadOnlyList<BuildingDetailsDto>>(buildings);
        }
    }
}
