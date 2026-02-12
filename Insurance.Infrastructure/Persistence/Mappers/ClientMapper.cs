using Insurance.Domain.Clients;
using Insurance.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Mappers
{
    [ExcludeFromCodeCoverage]
    public static class ClientMapper
    {
        public static Client ToDomain(ClientEntity entity)
        {
            return Client.Rehydrate(
                entity.Id,
                entity.BrokerId,
                Enum.Parse<ClientType>(entity.Type),
                entity.Name,
                entity.IdentificationNumber,
                entity.Email,
                entity.PhoneNumber,
                entity.Address);
        }

        public static ClientEntity ToEntity(Client domain)
            => new()
            {
                Id = domain.Id,
                BrokerId = domain.BrokerId,
                Type = domain.Type.ToString(),
                Name = domain.Name,
                IdentificationNumber = domain.IdentificationNumber,
                Email = domain.Email,
                PhoneNumber = domain.PhoneNumber,
                Address = domain.Address
            };


    }
}
