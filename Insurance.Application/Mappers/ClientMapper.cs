using AutoMapper;
using Insurance.Application.Clients.DTOs;
using Insurance.Domain.Clients;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Mappers
{
    public class ClientMapper : Profile
    {
        public ClientMapper()
        {
            CreateMap<CreateClientDto, Client>().ReverseMap();
            CreateMap<ClientDetailsDto, Client>().ReverseMap();
        }
    }
}
