using Insurance.Reporting.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;


namespace Insurance.Reporting.Infrastructure.Persistence
{
    public class ReportingDbContext : DbContext
    {
        public ReportingDbContext(DbContextOptions<ReportingDbContext> options)
       : base(options)
        {
        }

        public DbSet<PolicyReportAggregate> PolicyReportAggregates => Set<PolicyReportAggregate>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PolicyReportAggregate>(entity =>
            {
                entity.HasKey(x => x.PolicyId);

                entity.HasIndex(x => x.Country);
                entity.HasIndex(x => x.CreatedAt);
                entity.HasIndex(x => x.Status);
                entity.HasIndex(x => x.Currency);
            });
        }
    }
}
