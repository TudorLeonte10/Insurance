using Insurance.Application.Geography.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Geography.Queries
{
    public record GetCitiesByCountyQuery(Guid CountyId) : IRequest<IReadOnlyList<CityDto>>;
}
