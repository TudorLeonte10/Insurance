using AutoMapper;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Geography.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Geography.Queries
{
    public class GetCountiesByCountryQueryHandler : IRequestHandler<GetCountiesByCountryQuery, IReadOnlyList<CountyDto>>
    {
        private readonly IGeographyRepository _geographyRepository;
        private readonly IMapper _mapper;
        public GetCountiesByCountryQueryHandler(IGeographyRepository geographyRepository, IMapper mapper)
        {
            _geographyRepository = geographyRepository;
            _mapper = mapper;
        }
        public async Task<IReadOnlyList<CountyDto>> Handle(GetCountiesByCountryQuery request, CancellationToken cancellationToken)
        {
            var counties = await _geographyRepository.GetCountiesByCountryIdAsync(request.CountryId, cancellationToken);
            return _mapper.Map<IReadOnlyList<CountyDto>>(counties);
        }
    }
}
