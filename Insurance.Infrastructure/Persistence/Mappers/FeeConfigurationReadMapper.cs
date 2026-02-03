using AutoMapper;
using Insurance.Application.FeeConfiguration.DTOs;
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
            CreateMap<FeeConfigurationEntity, FeeConfigurationDto>();
        }
    }
}
