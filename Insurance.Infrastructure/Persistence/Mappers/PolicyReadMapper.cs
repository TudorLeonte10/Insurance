using AutoMapper;
using Insurance.Application.Policy.DTOs;
using Insurance.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Mappers
{
    public class PolicyReadMapper : Profile
    {
        public PolicyReadMapper()
        {
            CreateMap<PolicyEntity, PolicyDetailsDto>();
        }
    }
}
