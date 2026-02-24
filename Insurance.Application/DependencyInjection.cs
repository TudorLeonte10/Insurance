using AutoMapper;
using FluentValidation;
using Insurance.Application.Common.Behaviours;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Insurance.Application
{
    public static class DependencyInjection
    {
        [ExcludeFromCodeCoverage]
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {

            services.AddMediatR(typeof(DependencyInjection).Assembly);

            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            services.AddTransient(
                 typeof(IPipelineBehavior<,>),
                 typeof(LoggingBehaviour<,>));

            services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehaviour<,>));

            services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(BrokerValidationBehaviour<,>));

            return services;
        }
    }
}
