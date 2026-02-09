using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Audit.Models
{
    [ExcludeFromCodeCoverage]
    public class MongoAuditLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string EntityType { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.String)]
        public Guid EntityId { get; set; }

        public DateTime ChangedAt { get; set; }
        public string ChangedBy { get; set; } = string.Empty;

        public List<MongoAuditChange> Changes { get; set; } = new();
    }

    public class MongoAuditChange
    {
        public string Field { get; set; } = string.Empty;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
    }
}
