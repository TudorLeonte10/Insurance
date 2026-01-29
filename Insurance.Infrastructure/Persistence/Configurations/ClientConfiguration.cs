using Insurance.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Infrastructure.Persistence.Configurations;

public class ClientEntityConfiguration
    : IEntityTypeConfiguration<ClientEntity>
{
    public void Configure(EntityTypeBuilder<ClientEntity> builder)
    {
        builder.ToTable("Clients");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.IdentificationNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.Type)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Email)
            .HasMaxLength(200);

        builder.Property(c => c.PhoneNumber)
            .HasMaxLength(50);

        builder.Property(c => c.Address)
            .HasMaxLength(300);

        builder.HasIndex(c => c.IdentificationNumber)
            .IsUnique();

        builder.HasMany(c => c.Buildings)
            .WithOne(b => b.Client)
            .HasForeignKey(b => b.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
