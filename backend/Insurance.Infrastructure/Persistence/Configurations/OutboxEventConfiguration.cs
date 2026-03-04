using Insurance.Infrastructure.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Configurations
{
    public class OutboxEventConfiguration : IEntityTypeConfiguration<OutboxEvent>
    {
        public void Configure(EntityTypeBuilder<OutboxEvent> builder)
        {
            builder.ToTable("OutboxEvents");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.EventType)
                  .IsRequired();

            builder.Property(x => x.Payload)
                  .IsRequired();

            builder.HasIndex(x => x.Processed);
        }
    }
}
