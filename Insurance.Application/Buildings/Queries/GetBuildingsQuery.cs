using Insurance.Application.Buildings.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Buildings.Queries
{
    public class GetBuildingsQuery : IRequest<IReadOnlyList<BuildingDetailsDto>>
    {
        public Guid? BuildingId { get; init; }
        public Guid? ClientId { get; init; }
    }
}
