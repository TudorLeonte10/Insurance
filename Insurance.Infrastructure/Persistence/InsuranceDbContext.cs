using System;
using System.Collections.Generic;
using System.Text;

using Insurance.Domain.Buildings;
using Insurance.Domain.Clients;
using Insurance.Domain.Geography;
using Insurance.Domain.RiskIndicators;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Persistence
{
    public class InsuranceDbContext : DbContext
    {
        public InsuranceDbContext(DbContextOptions<InsuranceDbContext> options)
            : base(options)
        {
        }
        public DbSet<Country> Countries => Set<Country>();
        public DbSet<County> Counties => Set<County>();
        public DbSet<City> Cities => Set<City>();
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Building> Buildings => Set<Building>();
        public DbSet<BuildingRiskIndicator> RiskIndicators => Set<BuildingRiskIndicator>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InsuranceDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
