using Insurance.Application;
using Insurance.Infrastructure;
using Insurance.Infrastructure.Messaging.Rabbit;
using Insurance.Infrastructure.Persistence;
using Insurance.Infrastructure.Persistence.Outbox;
using Insurance.Infrastructure.Persistence.Seed;
using Insurance.Reporting.Infrastructure.Persistence;
using Insurance.WebApi.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: true));
                });

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddReportingDbContext(builder.Configuration);

builder.Services.AddSingleton<RabbitMqPublisher>();
builder.Services.AddHostedService<OutboxPublisherBackgroundService>();

if (!builder.Environment.IsEnvironment("Test"))
{
    builder.Services.AddSqlServerDbContext(builder.Configuration);
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Insurance API", 
        Version = "v1" 
    });

    c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("bearer", doc)] = []
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

    var connectionString = config.GetConnectionString("DefaultConnection");

    if (!env.IsEnvironment("Test") && !string.IsNullOrWhiteSpace(connectionString))
    {
        var context = scope.ServiceProvider.GetRequiredService<InsuranceDbContext>();

        await context.Database.MigrateAsync();

        var seeder = new DatabaseSeeder(context);
        await seeder.SeedAsync();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

