using Insurance.Domain.RiskIndicators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Configurations
{
    public class BuildingRiskIndicatorConfiguration : IEntityTypeConfiguration<BuildingRiskIndicator>
    {
        public void Configure(EntityTypeBuilder<BuildingRiskIndicator> builder)
        {
            builder.ToTable("BuildingRiskIndicators");

            builder.HasKey(x => new { x.BuildingId, x.RiskIndicator });

            builder.Property(x => x.RiskIndicator)
                .IsRequired();

            builder.HasOne(x => x.Building)
                .WithMany(b => b.RiskIndicators)
                .HasForeignKey(x => x.BuildingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
