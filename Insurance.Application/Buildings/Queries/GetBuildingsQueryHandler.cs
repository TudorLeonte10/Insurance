using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Buildings.DTOs;
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

        public GetBuildingsQueryHandler(
            IBuildingReadRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<BuildingDetailsDto>> Handle(
            GetBuildingsQuery request,
            CancellationToken cancellationToken)
        {
            if (request.BuildingId.HasValue)
            {
                var building = await _repository.GetByIdAsync(
                    request.BuildingId.Value,
                    cancellationToken);

                if (building is null)
                    throw new NotFoundException(
                        $"Building {request.BuildingId} not found.");

                return new[] { building };
            }

            if (request.ClientId.HasValue)
            {
                return await _repository.GetByClientIdAsync(
                    request.ClientId.Value,
                    cancellationToken);
            }
            
            throw new ArgumentException(
                "Either BuildingId or ClientId must be provided.");
        }
    }

}
