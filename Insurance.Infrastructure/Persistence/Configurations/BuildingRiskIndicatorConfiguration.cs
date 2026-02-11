using Insurance.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Insurance.Infrastructure.Persistence.Configurations;

[ExcludeFromCodeCoverage]
public class BuildingRiskIndicatorEntityConfiguration
    : IEntityTypeConfiguration<BuildingRiskIndicatorEntity>
{
    public void Configure(EntityTypeBuilder<BuildingRiskIndicatorEntity> builder)
    {
        builder.ToTable("BuildingRiskIndicators");

        builder.HasKey(ri => ri.Id);

        builder.Property(ri => ri.RiskIndicator)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(ri => ri.Building)
            .WithMany() 
            .HasForeignKey(ri => ri.BuildingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ri => ri.Building)
            .WithMany(b => b.RiskIndicators)
            .HasForeignKey(ri => ri.BuildingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ri => new { ri.BuildingId, ri.RiskIndicator })
            .IsUnique();
    }
}
