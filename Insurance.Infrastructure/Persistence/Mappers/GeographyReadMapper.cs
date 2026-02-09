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
        }
    }
}
