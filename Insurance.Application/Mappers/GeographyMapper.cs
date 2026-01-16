using AutoMapper;
using Insurance.Application.Geography.DTOs;
using Insurance.Domain.Geography;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Mappers
{
    public class GeographyMapper : Profile
    {
        public GeographyMapper()
        {
            CreateMap<CountryDto, Country>().ReverseMap();
            CreateMap<CountyDto, County>().ReverseMap();
            CreateMap<CityDto, City>().ReverseMap();
        }
    }
}
