using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence
{
    [ExcludeFromCodeCoverage]
    public class InsuranceDbContextFactory : IDesignTimeDbContextFactory<InsuranceDbContext>
    {
        public InsuranceDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<InsuranceDbContext>();
            optionsBuilder.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"));

            return new InsuranceDbContext(optionsBuilder.Options);
        }
    }
}
