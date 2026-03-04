using AutoMapper;
using Insurance.Application.Metadata.FeeConfiguration.DTOs;
using Insurance.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Mappers
{
    public class FeeConfigurationReadMapper : Profile
    {
        public FeeConfigurationReadMapper()
        {
            CreateMap<FeeConfigurationEntity, FeeConfigurationDto>()
                .ForMember(
            dest => dest.RiskIndicator,
            opt => opt.MapFrom(src => src.RiskIndicatorType)
        ); 
        }
    }
}
