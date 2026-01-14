using Insurance.Domain.Geography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Configurations.Geography
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("Countries");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(c => c.Counties)
                .WithOne(cn => cn.Country)
                .HasForeignKey(cn => cn.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
