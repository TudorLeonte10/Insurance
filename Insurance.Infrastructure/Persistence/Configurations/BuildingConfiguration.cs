using Insurance.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Insurance.Infrastructure.Persistence.Configurations;

[ExcludeFromCodeCoverage]
public class BuildingEntityConfiguration
    : IEntityTypeConfiguration<BuildingEntity>
{
    public void Configure(EntityTypeBuilder<BuildingEntity> builder)
    {
        builder.ToTable("Buildings");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Street)
            .HasMaxLength(200);

        builder.Property(b => b.Number)
            .HasMaxLength(20);

        builder.Property(b => b.Type)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(b => b.ConstructionYear)
            .IsRequired();

        builder.Property(b => b.NumberOfFloors)
            .IsRequired();

        builder.Property(b => b.SurfaceArea)
            .IsRequired();

        builder.Property(b => b.InsuredValue)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

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
