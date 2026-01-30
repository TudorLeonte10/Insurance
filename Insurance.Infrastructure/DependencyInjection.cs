using Insurance.Application.Abstractions;
using Insurance.Application.Abstractions.Audit;
using Insurance.Application.Abstractions.Loggers;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Domain.Brokers;
using Insurance.Domain.Buildings;
using Insurance.Domain.Clients;
using Insurance.Infrastructure.Audit;
using Insurance.Infrastructure.Loggers;
using Insurance.Infrastructure.Persistence;
using Insurance.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddAutoMapper(typeof(DependencyInjection).Assembly);

            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IGeographyReadRepository, GeographyReadRepository>();
            services.AddScoped<IBuildingRepository, BuildingRepository>();
            services.AddScoped<IBuildingReadRepository, BuildingReadRepository>();
            services.AddScoped<IClientReadRepository, ClientReadRepository>();
            services.AddScoped<IClientSearchRepository, ClientSearchRepository>();
            services.AddScoped<IBrokerRepository, BrokerRepository>();

            services.AddScoped<IApplicationLogger, ApplicationLogger>();
            services.AddScoped<IAuditLogger, AuditLogger>();

            services.AddSingleton<IMongoClient>(sp =>
            {
                var connectionString = configuration.GetConnectionString("MongoDb");
                return new MongoClient(connectionString);
            });

            services.AddScoped<IMongoDatabase>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase("insurance_audit");
            });

            services.AddScoped<IAuditLogService, MongoAuditLogService>();



            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddSqlServerDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<InsuranceDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));
            return services;
        }
    }
}
