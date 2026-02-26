using Insurance.Application.Abstractions.Audit;
using Insurance.Infrastructure.Audit.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Audit
{
    public class MongoAuditLogService : IAuditLogService
    {
        private readonly IMongoCollection<MongoAuditLog> _collection;

        public MongoAuditLogService(IMongoDatabase database)
        {
            _collection = database.GetCollection<MongoAuditLog>("insurance_audit");
        }

        public async Task LogAsync(AuditEntry entry, CancellationToken cancellationToken)
        {
            var document = new MongoAuditLog
            {
                EntityType = entry.EntityType,
                EntityId = entry.EntityId,
                ChangedAt = entry.ChangedAt,
                ChangedBy = entry.ChangedBy,
                Changes = entry.Changes
                    .Select(c => new MongoAuditChange
                    {
                        Field = c.Field,
                        OldValue = c.OldValue,
                        NewValue = c.NewValue
                    })
                    .ToList()
            };

            await _collection.InsertOneAsync(document, cancellationToken: cancellationToken);
        }
    }
}
