using Insurance.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Configurations
{
    [ExcludeFromCodeCoverage]
    public class BrokerEntityConfiguration : IEntityTypeConfiguration<BrokerEntity>
    {
        public void Configure(EntityTypeBuilder<BrokerEntity> builder)
        {
            builder.ToTable("Brokers");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.BrokerCode)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(b => b.BrokerCode)
                .IsUnique();

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.Phone)
                .HasMaxLength(20);

            builder.Property(b => b.IsActive)
                .HasDefaultValue(true);

            builder.Property(b => b.CreatedAt)
                .IsRequired();

            builder.HasMany(b => b.Policies)
                .WithOne(p => p.Broker)
                .HasForeignKey(p => p.BrokerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
