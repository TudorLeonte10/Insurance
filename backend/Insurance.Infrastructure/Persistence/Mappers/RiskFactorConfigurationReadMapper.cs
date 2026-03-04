using AutoMapper;
using Insurance.Application.Metadata.RiskFactors.DTOs;
using Insurance.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Mappers
{
    public class RiskFactorConfigurationReadMapper : Profile
    {
        public RiskFactorConfigurationReadMapper()
        {
            CreateMap<RiskFactorConfigurationEntity, RiskFactorConfigurationDto>();
        }

    }
}
