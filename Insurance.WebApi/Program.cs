using Insurance.Application;
using Insurance.Infrastructure;
using Insurance.Infrastructure.Persistence;
using Insurance.Infrastructure.Persistence.Seed;
using Insurance.WebApi.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

if (!builder.Environment.IsEnvironment("Test"))
{
    builder.Services.AddSqlServerDbContext(builder.Configuration);
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MapControllers();

app.Run();

