using Insurance.Application.Buildings.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Buildings.Queries
{
    public record GetBuildingsByClientQuery(Guid ClientId) : IRequest<IReadOnlyList<BuildingDetailsDto>>;
}
