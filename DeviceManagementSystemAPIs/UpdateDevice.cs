using DeviceManagementSystem.Application.Services;
using DeviceManagementSystem.Core.Entities;
using DeviceManagementSystem.Core.Interfaces;
using DeviceManagementSystem.Infrastructure.Data;
using DeviceManagementSystem.Infrastructure.Repositories;
using DeviceManagementSystem.Utilities.Connections;
using DeviceManagementSystem.Utilities.Validators;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DeviceManagementSystemFA
{
    public class UpdateDevice
    {
        private readonly ILogger _logger;

        public UpdateDevice(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function>();
        }

        [Function("UpdateDevice")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("UpdateDevice triggered.");

            // parse query parameter
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            Device? device = null;
            HttpResponseData response;

            try
            {
                device = JsonSerializer.Deserialize<Device>(requestBody);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"{ex.Message} Input that failed: {requestBody}");
                response = req.CreateResponse(HttpStatusCode.BadRequest);
                response.WriteString(ex.Message);
                return response;
            }

            if (device is null)
            {
                _logger.LogError($"Invalid device data. Input that failed: {requestBody}");
                response = req.CreateResponse(HttpStatusCode.BadRequest);
                response.WriteString("Invalid device data.");
                return response;
            }

            var connectionString = ConnectionStringHelper.GetCustomConnectionString("DeviceDatabase");
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Register DbContext
                    services.AddDbContext<DeviceContext>(options =>
                        options.UseSqlServer(connectionString));

                    // Register repositories and services
                    services.AddScoped<IDeviceRepository, DeviceRepository>();
                    services.AddScoped<DeviceService>();
                })
                .Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var deviceService = services.GetRequiredService<DeviceService>();
                DeviceValidator validator = new DeviceValidator(device);
                var validationResult = validator.Validate();


                if (validationResult.IsValid)
                {
                    try
                    {
                        // Check if device exist already.
                        Device currentDevice = deviceService.GetDeviceById(device.SerialNumber);
                        var changesAcceptable = ChangesAcceptable(currentDevice, device);

                        if (changesAcceptable.Acceptable)
                        {
                            deviceService.UpdateDevice(device);
                        }
                        else
                        {
                            _logger.LogInformation($"Invalid update request. {changesAcceptable.ErrorMessage}");
                            response = req.CreateResponse(HttpStatusCode.BadRequest);
                            response.WriteString($"Invalid update request. You cannot change the {changesAcceptable.InvalidField} field.");
                            return response;
                        }

                    }
                    catch (ArgumentException)
                    {
                        // GetDeviceById will throw an argument exception if the serial number does not exist in the database.
                        _logger.LogInformation("Device does not exist.");
                        response = req.CreateResponse(HttpStatusCode.BadRequest);
                        response.WriteString("Device does not exist.");
                        return response;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"An error occurred: {ex.Message}");
                        response = req.CreateResponse(HttpStatusCode.InternalServerError);
                        response.WriteString("An internal error has occured. Device was not added.");
                        return response;
                    }
                }
                else
                {
                    _logger.LogInformation($"Failed to validate device: {validationResult.ErrorMessage}");
                    response = req.CreateResponse(HttpStatusCode.BadRequest);
                    response.WriteString($"Failed to validate device: {validationResult.ErrorMessage}");
                    return response;
                }
            }

            response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteString("Device has been updated.");

            return response;
        }

        private (bool Acceptable, string? ErrorMessage, string? InvalidField) ChangesAcceptable(Device currentDevice, Device device)
        {
            if (currentDevice.ModelId != device.ModelId) { return (false, "Model ID has been attempted changed.", "ModelId"); }
            if (currentDevice.ModelName != device.ModelName) { return (false, "Model name has been attempted changed.", "ModelName"); }
            if (currentDevice.Manufacturer != device.Manufacturer) { return (false, "Manufacturer has been attempted changed.", "Manufacturer"); }
            if (currentDevice.DeviceType != device.DeviceType) { return (false, "DeviceType has been attempted changed.", "DeviceType"); }

            return (true, null, null);
        }
    }
}
