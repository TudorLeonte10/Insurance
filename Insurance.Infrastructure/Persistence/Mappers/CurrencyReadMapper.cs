using AutoMapper;
using Insurance.Application.Metadata.Currency.DTOs;
using Insurance.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Mappers
{
    public class CurrencyReadMapper : Profile
    {
        public CurrencyReadMapper()
        {
            CreateMap<CurrencyEntity, CurrencyDto>();
        }
    }
}
