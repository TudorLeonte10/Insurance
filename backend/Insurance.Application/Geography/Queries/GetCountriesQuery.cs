using Insurance.Application.Geography.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Geography.Queries
{
    public record GetCountriesQuery : IRequest<IReadOnlyList<CountryDto>>;

}
