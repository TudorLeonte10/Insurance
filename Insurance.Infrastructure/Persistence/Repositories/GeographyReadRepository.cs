using AutoMapper;
using AutoMapper.QueryableExtensions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Geography.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Repositories
{
    [ExcludeFromCodeCoverage]
    public class GeographyReadRepository : IGeographyReadRepository
    {
        private readonly InsuranceDbContext _context;
        private readonly IMapper _mapper;

        public GeographyReadRepository(
            InsuranceDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<CountryDto>> GetCountriesAsync(CancellationToken ct)
        {
            return await _context.Countries
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ProjectTo<CountryDto>(_mapper.ConfigurationProvider)
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<CountyDto>> GetCountiesByCountryIdAsync(
            Guid countryId,
            CancellationToken ct)
        {
            return await _context.Counties
                .AsNoTracking()
                .Where(c => c.CountryId == countryId)
                .OrderBy(c => c.Name)
                .ProjectTo<CountyDto>(_mapper.ConfigurationProvider)
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<CityDto>> GetCitiesByCountyIdAsync(
            Guid countyId,
            CancellationToken ct)
        {
            return await _context.Cities
                .AsNoTracking()
                .Where(c => c.CountyId == countyId)
                .OrderBy(c => c.Name)
                .ProjectTo<CityDto>(_mapper.ConfigurationProvider)
                .ToListAsync(ct);
        }

        public async Task<CityDto?> GetCityByIdAsync(Guid cityId, CancellationToken ct)
        {
            return await _context.Cities
                .AsNoTracking()
                .Where(c => c.Id == cityId)
                .ProjectTo<CityDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);
        }
    }

}
