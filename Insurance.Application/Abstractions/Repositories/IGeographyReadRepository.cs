using Insurance.Application.Geography.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Abstractions.Repositories
{
    public interface IGeographyReadRepository
    {
        Task<IReadOnlyList<CountryDto>> GetCountriesAsync(CancellationToken ct);
        Task<IReadOnlyList<CountyDto>> GetCountiesByCountryIdAsync(Guid countryId, CancellationToken ct);
        Task<IReadOnlyList<CityDto>> GetCitiesByCountyIdAsync(Guid countyId, CancellationToken ct);

        Task<CityDto?> GetCityByIdAsync(Guid cityId, CancellationToken ct);
    }
}
