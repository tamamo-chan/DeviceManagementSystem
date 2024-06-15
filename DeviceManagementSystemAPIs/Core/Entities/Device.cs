using DeviceManagementSystem.Utilities.JsonConverts;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DeviceManagementSystem.Core.Entities
{
    public class Device
    {
        [Key]
        public Guid SerialNumber { get; init; }

        [Required]
        public string ModelId { get; init; }

        [Required]
        public string ModelName { get; init; }

        [Required]
        public string Manufacturer { get; init; }

        [EmailAddress]
        public string? PrimaryUser { get; set; }

        public string? OperatingSystem { get; set; }

        [Required]
        [EnumDataType(typeof(DeviceType))]
        [JsonConverter(typeof(DeviceTypeConverter))]
        public DeviceType DeviceType { get; set; }

        [Required]
        [EnumDataType(typeof(DeviceStatus))]
        [JsonConverter(typeof(DeviceStatusConverter))]
        public DeviceStatus Status { get; set; }
    }

    public enum DeviceType
    {
        Laptop,
        Desktop
    }

    public enum DeviceStatus
    {
        Active,
        Inactive,
        Retired
    }
}
