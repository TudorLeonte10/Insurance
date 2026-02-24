using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Reporting.Infrastructure.Persistence
{
    public static class ReportingDbContextExtensions
    {
        public static IServiceCollection AddReportingDbContext(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ReportingDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("ReportingDb")));

            return services;
        }
    }
}
