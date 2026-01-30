using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MediatR;

namespace Insurance.Tests.Integration;

public class ControllerTestWebApplicationFactory
    : WebApplicationFactory<Program>
{
    public Mock<IMediator> MediatorMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.Single(
                d => d.ServiceType == typeof(IMediator));

            services.Remove(descriptor);


            services.AddSingleton<IMediator>(MediatorMock.Object);
        });
    }
}
