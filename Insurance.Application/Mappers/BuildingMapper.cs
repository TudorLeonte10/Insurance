using AutoMapper;
using Insurance.Application.Buildings.DTOs;
using Insurance.Domain.Buildings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Mappers
{
    public class BuildingMapper : Profile
    {
        public BuildingMapper()
        {
            CreateMap<Building, BuildingDetailsDto>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.County, opt => opt.MapFrom(src => src.City.County.Name))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.City.County.Country.Name))
                .ForMember(dest => dest.RiskIndicators, opt => opt.MapFrom(src => src.RiskIndicators.Select(ri => ri.RiskIndicator.ToString())));
        }
    }
}
