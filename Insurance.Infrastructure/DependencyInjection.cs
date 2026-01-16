using Insurance.Application.Abstractions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Domain.Abstractions.Repositories;
using Insurance.Infrastructure.Persistence;
using Insurance.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<InsuranceDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IGeographyRepository, GeographyRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
