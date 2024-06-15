using DeviceManagementSystem.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace DeviceManagementSystem.Utilities.Validators
{
    public class DeviceValidator
    {
        public Device Device { get; set; }

        public DeviceValidator(Device device)
        {
            Device = device;
        }

        public (bool IsValid, string? ErrorMessage) Validate()
        {
            if (Device.SerialNumber == Guid.Empty)
            {
                return (false, "Invalid or missing SerialNumber.");
            }

            if (string.IsNullOrWhiteSpace(Device.ModelId))
            {
                return (false, "ModelId is required.");
            }

            if (string.IsNullOrWhiteSpace(Device.ModelName))
            {
                return (false, "ModelName is required.");
            }

            if (string.IsNullOrWhiteSpace(Device.Manufacturer))
            {
                return (false, "Manufacturer is required.");
            }

            if (!string.IsNullOrWhiteSpace(Device.PrimaryUser))
            {
                if (!new EmailAddressAttribute().IsValid(Device.PrimaryUser))
                {
                    return (false, "PrimaryUser email is invalid.");
                }
            }

            if (!Enum.IsDefined(typeof(DeviceType), Device.DeviceType))
            {
                return (false, "Invalid DeviceType.");
            }

            if (!Enum.IsDefined(typeof(DeviceStatus), Device.Status))
            {
                return (false, "Invalid DeviceStatus.");
            }

            return (true, null);
        }
    }
}
