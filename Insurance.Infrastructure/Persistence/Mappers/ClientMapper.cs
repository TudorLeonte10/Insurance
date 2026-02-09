using Insurance.Domain.Clients;
using Insurance.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Mappers
{
    public static class ClientMapper
    {
        public static Client ToDomain(ClientEntity entity)
        {
            var client = Client.Create(
                Enum.Parse<ClientType>(entity.Type),
                entity.Name,
                entity.IdentificationNumber,
                entity.Email,
                entity.PhoneNumber,
                entity.Address);

            typeof(Client)
                .GetProperty(nameof(Client.Id))!
                .SetValue(client, entity.Id);

            return client;
        }

        public static ClientEntity ToEntity(Client domain)
            => new()
            {
                Id = domain.Id,
                Type = domain.Type.ToString(),
                Name = domain.Name,
                IdentificationNumber = domain.IdentificationNumber,
                Email = domain.Email,
                PhoneNumber = domain.PhoneNumber,
                Address = domain.Address
            };
    }
}
