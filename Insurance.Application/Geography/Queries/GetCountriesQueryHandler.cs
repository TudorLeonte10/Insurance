using AutoMapper;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Geography.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Geography.Queries
{
    public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, IReadOnlyList<CountryDto>>
    {
        private readonly IGeographyRepository _geographyRepository;
        private readonly IMapper _mapper;
        public GetCountriesQueryHandler(IGeographyRepository geographyRepository, IMapper mapper)
        {
            _geographyRepository = geographyRepository;
            _mapper = mapper;
        }
        public async Task<IReadOnlyList<CountryDto>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            var countries =  await _geographyRepository.GetAllCountriesAsync(cancellationToken);
            return _mapper.Map<IReadOnlyList<CountryDto>>(countries);
        }
    }
}
