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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<InsuranceDbContext>();
    var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

    await context.Database.MigrateAsync();

    await DatabaseSeeder.SeedAsync(context, env);
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

