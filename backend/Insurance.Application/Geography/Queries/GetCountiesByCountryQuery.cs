using Insurance.Application.Geography.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Geography.Queries
{
    public record GetCountiesByCountryQuery(Guid CountryId) : IRequest<IReadOnlyList<CountyDto>>;
}
