using Insurance.Domain.Buildings;
using Insurance.Domain.Clients;
using Insurance.Domain.RiskIndicators;
using Insurance.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence
{
    [ExcludeFromCodeCoverage]
    public class InsuranceDbContext : DbContext
    {
        public InsuranceDbContext(DbContextOptions<InsuranceDbContext> options)
            : base(options)
        {
        }
        public DbSet<ClientEntity> Clients => Set<ClientEntity>();
        public DbSet<BuildingEntity> Buildings => Set<BuildingEntity>();
        public DbSet<CityEntity> Cities => Set<CityEntity>();
        public DbSet<CountyEntity> Counties => Set<CountyEntity>();
        public DbSet<CountryEntity> Countries => Set<CountryEntity>();
        public DbSet<BuildingRiskIndicatorEntity> BuildingRiskIndicators => Set<BuildingRiskIndicatorEntity>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InsuranceDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
