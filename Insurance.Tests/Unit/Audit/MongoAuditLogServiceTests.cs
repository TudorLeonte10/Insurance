using Insurance.Application.Abstractions.Audit;
using Insurance.Infrastructure.Audit;
using Insurance.Infrastructure.Audit.Models;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.Audit
{
    public class MongoAuditLogServiceTests
    {
        private readonly Mock<IMongoCollection<MongoAuditLog>> _collectionMock;
        private readonly Mock<IMongoDatabase> _databaseMock;
        private readonly MongoAuditLogService _service;

        public MongoAuditLogServiceTests()
        {
            _collectionMock = new Mock<IMongoCollection<MongoAuditLog>>();
            _databaseMock = new Mock<IMongoDatabase>();

            _databaseMock
                .Setup(db => db.GetCollection<MongoAuditLog>(
                    It.IsAny<string>(),
                    It.IsAny<MongoCollectionSettings>()))
                .Returns(_collectionMock.Object);

            _service = new MongoAuditLogService(_databaseMock.Object);
        }

        [Fact]
        public async Task LogAsync_Should_Insert_Audit_Log_Document()
        {
            var auditEntry = new AuditEntry
            {
                EntityType = "Building",
                EntityId = Guid.NewGuid(),
                ChangedAt = DateTime.UtcNow,
                ChangedBy = "unit-test",
                Changes = new List<AuditChangeEntry>
                {
                    new AuditChangeEntry("Street", "Old Street", "New Street"),
                    new AuditChangeEntry("InsuredValue", "100000", "120000")
                }
            };

            await _service.LogAsync(auditEntry, CancellationToken.None);

            _collectionMock.Verify(
                c => c.InsertOneAsync(
                    It.Is<MongoAuditLog>(doc =>
                        doc.EntityType == auditEntry.EntityType &&
                        doc.EntityId == auditEntry.EntityId &&
                        doc.ChangedBy == auditEntry.ChangedBy &&
                        doc.Changes.Count == 2 &&
                        doc.Changes[0].Field == "Street" &&
                        doc.Changes[0].OldValue == "Old Street" &&
                        doc.Changes[0].NewValue == "New Street"
                    ),
                    null,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task LogAsync_Should_Handle_Empty_Changes_List()
        {
            var auditEntry = new AuditEntry
            {
                EntityType = "Building",
                EntityId = Guid.NewGuid(),
                ChangedAt = DateTime.UtcNow,
                ChangedBy = "unit-test",
                Changes = new List<AuditChangeEntry>()
            };

            await _service.LogAsync(auditEntry, CancellationToken.None);

            _collectionMock.Verify(
                c => c.InsertOneAsync(
                    It.Is<MongoAuditLog>(doc =>
                        doc.EntityType == auditEntry.EntityType &&
                        doc.EntityId == auditEntry.EntityId &&
                        doc.Changes.Count == 0
                    ),
                    null,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task LogAsync_Should_Not_Throw_When_CancellationToken_Is_Used()
        {
            var cts = new CancellationTokenSource();

            var auditEntry = new AuditEntry
            {
                EntityType = "Building",
                EntityId = Guid.NewGuid(),
                ChangedAt = DateTime.UtcNow,
                ChangedBy = "unit-test",
                Changes = new List<AuditChangeEntry>
                {
                    new AuditChangeEntry("Street", "A", "B")
                }
            };

            var exception = await Record.ExceptionAsync(() =>
                _service.LogAsync(auditEntry, cts.Token));

            Assert.Null(exception);
        }
    }
}
