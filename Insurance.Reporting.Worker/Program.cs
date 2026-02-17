using Insurance.Reporting.Infrastructure.Persistence;
using Insurance.Reporting.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContext<ReportingDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ReportingConnection")));


var host = builder.Build();
host.Run();
