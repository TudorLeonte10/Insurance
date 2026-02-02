using Insurance.Domain.Brokers;
using Insurance.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Mappers
{
    public static class BrokerMapper 
    {
        public static Broker ToDomain(BrokerEntity brokerEntity)
        {
            return Broker.Rehydrate(
                brokerEntity.Id,
                brokerEntity.BrokerCode,
                brokerEntity.Name,
                brokerEntity.Email,
                brokerEntity.Phone,
                brokerEntity.IsActive);
        }


        public static BrokerEntity ToEntity(Broker broker)
        {
            var brokerEntity = new BrokerEntity
            {
                Id = broker.Id,
                BrokerCode = broker.BrokerCode,
                Name = broker.Name,
                Email = broker.Email,
                Phone = broker.Phone,
                IsActive = broker.IsActive,
            };
            return brokerEntity;
        }

    }
}
