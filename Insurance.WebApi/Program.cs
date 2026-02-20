using Insurance.Application;
using Insurance.Infrastructure;
using Insurance.Infrastructure.Messaging.Rabbit;
using Insurance.Infrastructure.Persistence;
using Insurance.Infrastructure.Persistence.Outbox;
using Insurance.Infrastructure.Persistence.Seed;
using Insurance.Reporting.Infrastructure.Persistence;
using Insurance.WebApi.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: true));
                });

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);


if (!builder.Environment.IsEnvironment("Test"))
{
    builder.Services.AddSqlServerDbContext(builder.Configuration);
    builder.Services.AddReportingDbContext(builder.Configuration);
    builder.Services.AddSingleton<RabbitMqPublisher>();
    builder.Services.AddHostedService<OutboxPublisherBackgroundService>();
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("admin", new OpenApiInfo 
    { 
        Title = "Admin API", 
        Version = "v1" 
    });

    c.SwaggerDoc("broker", new OpenApiInfo
    {
        Title = "Broker API",
        Version = "v1"
    });

    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        var groupName = apiDesc.GroupName;

        if (string.IsNullOrEmpty(groupName))
            return docName == "admin" || docName == "broker";

        return groupName == docName;
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
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/admin/swagger.json", "Admin API");
        c.SwaggerEndpoint("/swagger/broker/swagger.json", "Broker API");
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<TraceMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();

