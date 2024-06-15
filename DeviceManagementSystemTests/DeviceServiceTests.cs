using DeviceManagementSystem.Application.Services;
using DeviceManagementSystem.Core.Entities;
using DeviceManagementSystem.Infrastructure.Data;
using DeviceManagementSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagementSystemTests
{
    public class DeviceServiceTests
    {
        private DbContextOptions<DeviceContext> GetInMemoryDbContextOptions()
        {
            return new DbContextOptionsBuilder<DeviceContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private DeviceService GetDeviceService(DeviceContext context)
        {
            var repository = new DeviceRepository(context);
            return new DeviceService(repository);
        }

        [Fact]
        public void AddDevice_ShouldAddDevice()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions();
            using var context = new DeviceContext(options);
            var service = GetDeviceService(context);
            var device = new Device
            {
                SerialNumber = Guid.NewGuid(),
                ModelId = "A123",
                ModelName = "Latitude 5490",
                Manufacturer = "Dell",
                PrimaryUser = "user@example.com",
                OperatingSystem = "Windows 10",
                DeviceType = DeviceType.Laptop,
                Status = DeviceStatus.Active
            };

            // Act
            service.AddDevice(device);
            var devices = service.GetAllDevices();

            // Assert
            Assert.Single(devices);
            Assert.Equal("Latitude 5490", devices.First().ModelName);
        }

        [Fact]
        public void UpdateDevice_ShouldUpdateDeviceStatus()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions();
            using var context = new DeviceContext(options);
            var service = GetDeviceService(context);
            var device = new Device
            {
                SerialNumber = Guid.NewGuid(),
                ModelId = "A123",
                ModelName = "Latitude 5490",
                Manufacturer = "Dell",
                PrimaryUser = "user@example.com",
                OperatingSystem = "Windows 10",
                DeviceType = DeviceType.Laptop,
                Status = DeviceStatus.Active
            };
            service.AddDevice(device);

            // Act
            device.Status = DeviceStatus.Inactive;
            service.UpdateDevice(device);
            var updatedDevice = service.GetDeviceById(device.SerialNumber);

            // Assert
            Assert.NotNull(updatedDevice);
            Assert.Equal(DeviceStatus.Inactive, updatedDevice.Status);
        }

        [Fact]
        public void DeleteDevice_ShouldRemoveDevice()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions();
            using var context = new DeviceContext(options);
            var service = GetDeviceService(context);
            var device = new Device
            {
                SerialNumber = Guid.NewGuid(),
                ModelId = "A123",
                ModelName = "Latitude 5490",
                Manufacturer = "Dell",
                PrimaryUser = "user@example.com",
                OperatingSystem = "Windows 10",
                DeviceType = DeviceType.Laptop,
                Status = DeviceStatus.Active
            };
            service.AddDevice(device);

            // Act
            service.DeleteDevice(device.SerialNumber);
            var devices = service.GetAllDevices();

            // Assert
            Assert.Empty(devices);
        }
    }
}
