using AutoMapper;
using Insurance.Application.Clients.DTOs;
using Insurance.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Mappers
{
    public class ClientReadMapper : Profile
    {
        public ClientReadMapper()
        {
            CreateMap<ClientEntity, ClientDetailsDto>();
        }
    }
}
