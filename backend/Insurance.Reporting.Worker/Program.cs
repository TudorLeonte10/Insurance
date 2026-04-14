using Insurance.Application.Abstractions.Repositories;
using Insurance.Infrastructure;
using Insurance.Infrastructure.Messaging.Rabbit;
using Insurance.Infrastructure.Reports;
using Insurance.Reporting.Infrastructure.Persistence;
using Insurance.Reporting.Infrastructure.Persistence.Repositories;
using Insurance.Reporting.Worker;
using Insurance.Reporting.Worker.Consumer;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContext<ReportingDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ReportingConnection")));

builder.Services.AddSingleton<RabbitMqPublisher>();

builder.Services.AddHostedService<PolicyCreatedConsumerBackgroundService>();
builder.Services.AddScoped<IPolicyIntegrationEventHandler, PolicyIntegrationEventHandler>();

builder.Services.AddScoped<IPolicyReportRepository, PolicyReportRepository>();
builder.Services.AddScoped<IPolicyReportGrouping, CountryReportGrouping>();
builder.Services.AddScoped<IPolicyReportGrouping, CountyReportGrouping>();
builder.Services.AddScoped<IPolicyReportGrouping, CityReportGrouping>();
builder.Services.AddScoped<IPolicyReportGrouping, BrokerReportGrouping>();
builder.Services.AddScoped<IPolicyReportGrouping, StatusReportGrouping>();
builder.Services.AddScoped<IPolicyReportGrouping, BuildingTypeReportGrouping>();

var host = builder.Build();
host.Run();
