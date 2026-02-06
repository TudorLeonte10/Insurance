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
            .ForMember(d => d.Client, opt => opt.MapFrom(s => s.Client))
            .ForMember(d => d.Building, opt => opt.MapFrom(s => s.Building))
            .ForMember(d => d.Currency, opt => opt.MapFrom(s => s.Currency))
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status));



        }

    }
}

