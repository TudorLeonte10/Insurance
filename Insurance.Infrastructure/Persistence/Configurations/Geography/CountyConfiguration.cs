using Insurance.Domain.Geography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Configurations.Geography
{
    public class CountyConfiguration : IEntityTypeConfiguration<County>
    {
        public void Configure(EntityTypeBuilder<County> builder)
        {
            builder.ToTable("Counties");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(c => c.Country)
                .WithMany(cn => cn.Counties)
                .HasForeignKey(c => c.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Cities)
                .WithOne(ci => ci.County)
                .HasForeignKey(ci => ci.CountyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
