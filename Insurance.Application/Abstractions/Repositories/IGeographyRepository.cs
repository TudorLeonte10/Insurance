using Insurance.Domain.Geography;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Abstractions.Repositories
{
    public interface IGeographyRepository
    {
        Task<IReadOnlyList<Country>> GetAllCountriesAsync(CancellationToken cancellationToken);
        Task<IReadOnlyList<County>> GetCountiesByCountryIdAsync(Guid countryId, CancellationToken cancellationToken);
        Task<IReadOnlyList<City>> GetCitiesByCountyIdAsync(Guid countyId, CancellationToken cancellationToken);
        Task<bool> ExistsCityAsync(Guid cityId, CancellationToken cancellationToken);
    }
}
