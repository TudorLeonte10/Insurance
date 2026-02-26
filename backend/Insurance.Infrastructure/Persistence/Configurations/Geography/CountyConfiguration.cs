using Insurance.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Insurance.Infrastructure.Persistence.Configurations;

[ExcludeFromCodeCoverage]
public class CountyEntityConfiguration
    : IEntityTypeConfiguration<CountyEntity>
{
    public void Configure(EntityTypeBuilder<CountyEntity> builder)
    {
        builder.ToTable("Counties");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(c => c.Country)
            .WithMany(cn => cn.Counties)
            .HasForeignKey(c => c.CountryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => new { c.CountryId, c.Name })
            .IsUnique();
    }
}
