using AutoMapper;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Geography.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Geography.Queries
{
    public class GetCitiesByCountyQueryHandler : IRequestHandler<GetCitiesByCountyQuery, IReadOnlyList<CityDto>>
    {
        private readonly IGeographyReadRepository _geographyRepository;
        private readonly IMapper _mapper;
        public GetCitiesByCountyQueryHandler(IGeographyReadRepository geographyRepository, IMapper mapper)
        {
            _geographyRepository = geographyRepository;
            _mapper = mapper;
        }
        public async Task<IReadOnlyList<CityDto>> Handle(GetCitiesByCountyQuery request, CancellationToken cancellationToken)
        {
            var cities = await _geographyRepository.GetCitiesByCountyIdAsync(request.CountyId, cancellationToken);
            return _mapper.Map<IReadOnlyList<CityDto>>(cities);
        }
    }
}
