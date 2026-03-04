using Insurance.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Insurance.Infrastructure.Persistence.Configurations;

[ExcludeFromCodeCoverage]
public class CityEntityConfiguration
    : IEntityTypeConfiguration<CityEntity>
{
    public void Configure(EntityTypeBuilder<CityEntity> builder)
    {
        builder.ToTable("Cities");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(c => c.County)
            .WithMany(ct => ct.Cities)
            .HasForeignKey(c => c.CountyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => new { c.CountyId, c.Name })
            .IsUnique();
    }
}
