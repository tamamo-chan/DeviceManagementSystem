

using DeviceManagementSystem.Core.Entities;
using DeviceManagementSystem.Utilities.Validators;

namespace DeviceManagementSystemTests
{
    public class DeviceValidatorTests
    {
        [Theory]
        [InlineData("d9bfb5ff-89d4-4e8c-bb3d-51d1d79e3b8c", "A123", "Latitude 5490", "Dell", "user@example.com", "Windows 10", DeviceType.Laptop, DeviceStatus.Active, true)]
        [InlineData("", "A123", "Latitude 5490", "Dell", "user@example.com", "Windows 10", DeviceType.Laptop, DeviceStatus.Active, false)]
        [InlineData("d9bfb5ff-89d4-4e8c-bb3d-51d1d79e3b8c", "", "Latitude 5490", "Dell", "user@example.com", "Windows 10", DeviceType.Laptop, DeviceStatus.Active, false)]
        [InlineData("d9bfb5ff-89d4-4e8c-bb3d-51d1d79e3b8c", "A123", "", "Dell", "user@example.com", "Windows 10", DeviceType.Laptop, DeviceStatus.Active, false)]
        [InlineData("d9bfb5ff-89d4-4e8c-bb3d-51d1d79e3b8c", "A123", "Latitude 5490", "", "user@example.com", "Windows 10", DeviceType.Laptop, DeviceStatus.Active, false)]
        [InlineData("d9bfb5ff-89d4-4e8c-bb3d-51d1d79e3b8c", "A123", "Latitude 5490", "Dell", "invalid-email", "Windows 10", DeviceType.Laptop, DeviceStatus.Active, false)]
        [InlineData("d9bfb5ff-89d4-4e8c-bb3d-51d1d79e3b8c", "A123", "Latitude 5490", "Dell", "", "Windows 10", DeviceType.Laptop, DeviceStatus.Active, true)]
        [InlineData("d9bfb5ff-89d4-4e8c-bb3d-51d1d79e3b8c", "A123", "Latitude 5490", "Dell", "user@example.com", "", DeviceType.Laptop, DeviceStatus.Active, true)]
        [InlineData("d9bfb5ff-89d4-4e8c-bb3d-51d1d79e3b8c", "A123", "Latitude 5490", "Dell", "user@example.com", "Windows 10", (DeviceType)99, DeviceStatus.Active, false)]
        [InlineData("d9bfb5ff-89d4-4e8c-bb3d-51d1d79e3b8c", "A123", "Latitude 5490", "Dell", "user@example.com", "Windows 10", DeviceType.Laptop, (DeviceStatus)99, false)]
        public void ValidateDeviceTests(
            string serialNumber, string modelId, string modelName, string manufacturer, string primaryUser, string operatingSystem, DeviceType deviceType, DeviceStatus deviceStatus, bool expectedIsValid)
        {
            // Arrange
            var device = new Device
            {
                SerialNumber = string.IsNullOrEmpty(serialNumber) ? Guid.Empty : Guid.Parse(serialNumber),
                ModelId = modelId,
                ModelName = modelName,
                Manufacturer = manufacturer,
                PrimaryUser = primaryUser,
                OperatingSystem = operatingSystem,
                DeviceType = deviceType,
                Status = deviceStatus
            };
            var validator = new DeviceValidator(device);

            // Act
            var validationResult = validator.Validate();

            // Assert
            Assert.Equal(expectedIsValid, validationResult.IsValid);
        }
    }
}