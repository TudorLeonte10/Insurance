using AutoMapper;
using Insurance.Application.Geography.DTOs;
using Insurance.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Mappers
{
    public class GeographyReadMapper : Profile
    {
        public GeographyReadMapper()
        {
            CreateMap<CountryEntity, CountryDto>();
            CreateMap<CountyEntity, CountyDto>();
            CreateMap<CityEntity, CityDto>();

            CreateMap<CityEntity, GeographyDto>()
            .ForMember(d => d.City, opt => opt.MapFrom(s => s.Name))
            .ForMember(d => d.County, opt => opt.MapFrom(s => s.County.Name))
            .ForMember(d => d.Country, opt => opt.MapFrom(s => s.County.Country.Name));
        }
    }
}
