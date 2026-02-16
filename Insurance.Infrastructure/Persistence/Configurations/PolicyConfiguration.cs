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
    public class PolicyEntityConfiguration : IEntityTypeConfiguration<PolicyEntity>
    {
        public void Configure(EntityTypeBuilder<PolicyEntity> builder)
        {
            builder.ToTable("Policies");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.PolicyNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(p => p.PolicyNumber)
                .IsUnique();

            builder.Property(p => p.Status)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(p => p.BasePremium)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.FinalPremium)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.StartDate)
                .IsRequired();

            builder.Property(p => p.EndDate)
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.Property(p => p.ActivatedAt)
                .IsRequired(false);

            builder.Property(p => p.CancelledAt)
                .IsRequired(false);

            builder.Property(p => p.CancellationReason)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.HasOne(p => p.Client)
                .WithMany(c => c.Policies)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Building)
                .WithMany(b => b.Policies)
                .HasForeignKey(p => p.BuildingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Broker)
                .WithMany(b => b.Policies)
                .HasForeignKey(p => p.BrokerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Currency)
                .WithMany(c => c.Policies)
                .HasForeignKey(p => p.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => new { p.Status, p.CurrencyId, p.StartDate, p.EndDate })
                .HasDatabaseName("IX_Policies_Report")
                .IncludeProperties(p => new { p.FinalPremium, p.BuildingId, p.BrokerId });
        }
    }

}
