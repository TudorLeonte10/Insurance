using Insurance.Application.Abstractions.Repositories;
using Insurance.Domain.Geography;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Repositories
{
    public class GeographyRepository : IGeographyRepository
    {
        private readonly InsuranceDbContext _context;

        public GeographyRepository(InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Country>> GetAllCountriesAsync(CancellationToken cancellationToken)
        {
            return await _context.Countries
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<City>> GetCitiesByCountyIdAsync(Guid countyId, CancellationToken cancellationToken)
        {
            return await _context.Cities
                .AsNoTracking()
                .Where(c => c.CountyId == countyId)
                .OrderBy(c => c.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<County>> GetCountiesByCountryIdAsync(Guid countryId, CancellationToken cancellationToken)
        {
            return await _context.Counties
                .AsNoTracking()
                .Where(c => c.CountryId == countryId)
                .OrderBy(c => c.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsCityAsync(Guid cityId, CancellationToken cancellationToken)
        {
            return await _context.Cities
                .AsNoTracking()
                .AnyAsync(c => c.Id == cityId, cancellationToken);
        }
    }
}
