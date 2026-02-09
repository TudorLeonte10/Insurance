using Insurance.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Infrastructure.Persistence.Configurations;

public class CountryEntityConfiguration
    : IEntityTypeConfiguration<CountryEntity>
{
    public void Configure(EntityTypeBuilder<CountryEntity> builder)
    {
        builder.ToTable("Countries");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(c => c.Name)
            .IsUnique();
    }
}
