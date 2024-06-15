using System.Text.Json.Serialization;
using System.Text.Json;
using DeviceManagementSystem.Core.Entities;

namespace DeviceManagementSystem.Utilities.JsonConverts
{
    public class DeviceStatusConverter : JsonConverter<DeviceStatus>
    {
        public override DeviceStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                var stringValue = reader.GetString();
                if (Enum.TryParse<DeviceStatus>(stringValue, true, out var deviceStatus))
                {
                    return deviceStatus;
                }
            }
            catch (Exception)
            {
                var intValue = reader.GetInt32();
                return (DeviceStatus)intValue;
            }
            throw new JsonException("Unable to convert to DeviceStatus");
        }

        public override void Write(Utf8JsonWriter writer, DeviceStatus value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
