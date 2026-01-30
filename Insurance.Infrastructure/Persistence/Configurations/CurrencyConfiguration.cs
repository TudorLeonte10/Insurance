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
    public class CurrencyEntityConfiguration : IEntityTypeConfiguration<CurrencyEntity>
    {
        public void Configure(EntityTypeBuilder<CurrencyEntity> builder)
        {
            builder.ToTable("Currencies");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Code)
                .IsRequired()
                .HasMaxLength(10);

            builder.HasIndex(c => c.Code)
                .IsUnique();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.ExchangeRateToBase)
                .IsRequired()
                .HasColumnType("decimal(18,6)");

            builder.Property(c => c.IsActive)
                .HasDefaultValue(true);
        }
    }

}
