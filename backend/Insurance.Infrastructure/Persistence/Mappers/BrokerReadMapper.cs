using AutoMapper;
using Insurance.Application.Brokers.DTOs;
using Insurance.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Mappers
{
    public class BrokerReadMapper : Profile
    {
        public BrokerReadMapper()
        {
            CreateMap<BrokerEntity, BrokerDetailsDto>();
        }
    }
}
