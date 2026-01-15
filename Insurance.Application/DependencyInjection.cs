using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {

            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(
                    typeof(DependencyInjection).Assembly));

            services.AddAutoMapper(
                typeof(DependencyInjection).Assembly);

            return services;
        }
    }
}
