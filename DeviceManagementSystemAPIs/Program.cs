using DeviceManagementSystem.Application.Services;
using DeviceManagementSystem.Core.Interfaces;
using DeviceManagementSystem.Infrastructure.Data;
using DeviceManagementSystem.Infrastructure.Repositories;
using DeviceManagementSystem.Utilities.Connections;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        var connectionString = ConnectionStringHelper.GetCustomConnectionString("DeviceDatabase");
        services.AddDbContext<DeviceContext>(options =>
                    options.UseSqlServer(connectionString));

        // Register other services
        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<DeviceService>();
    })
    .Build();

host.Run();
