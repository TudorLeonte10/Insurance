using AutoMapper;
using Insurance.Application.Policy.DTOs;
using Insurance.Domain.Policies;
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
            CreateMap<PolicyEntity, PolicyDetailsDto>()
    .ForMember(d => d.ClientId, opt => opt.MapFrom(s => s.ClientId))
    .ForMember(d => d.BuildingId, opt => opt.MapFrom(s => s.BuildingId))
    .ForMember(d => d.BrokerId, opt => opt.MapFrom(s => s.BrokerId))
    .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status));



        }

    }
}

