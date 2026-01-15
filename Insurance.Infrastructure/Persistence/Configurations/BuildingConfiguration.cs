using Insurance.Domain.Buildings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Configurations
{
    public class BuildingConfiguration : IEntityTypeConfiguration<Building>
    {
        public void Configure(EntityTypeBuilder<Building> builder)
        {
            builder.ToTable("Buildings");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Street)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(b => b.Number)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(b => b.ConstructionYear)
                .IsRequired();

            builder.Property(b => b.SurfaceArea)
                .IsRequired();

            builder.Property(b => b.InsuredValue)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(b => b.NumberOfFloors)
                .IsRequired();

            builder.Property(b => b.Type)
                .IsRequired();

            builder.HasOne(b => b.Client)
                .WithMany(c => c.Buildings)
                .HasForeignKey(b => b.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.City)
                .WithMany(c => c.Buildings)
                .HasForeignKey(b => b.CityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
