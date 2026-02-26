using Insurance.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Insurance.Infrastructure.Persistence.Configurations;

[ExcludeFromCodeCoverage]
public class ClientEntityConfiguration : IEntityTypeConfiguration<ClientEntity>
{
    public void Configure(EntityTypeBuilder<ClientEntity> builder)
    {
        builder.ToTable("Clients");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(c => c.Broker)
            .WithMany()
            .HasForeignKey(c => c.BrokerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(c => c.IdentificationNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(c => c.IdentificationNumber)
            .IsUnique();

        builder.Property(c => c.Type)
            .HasConversion<string>()
            .IsRequired();

        builder.HasMany(c => c.Policies)
            .WithOne(p => p.Client)
            .HasForeignKey(p => p.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}


