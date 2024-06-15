using System.Text.Json.Serialization;
using System.Text.Json;
using DeviceManagementSystem.Core.Entities;

namespace DeviceManagementSystem.Utilities.JsonConverts
{
    public class DeviceTypeConverter : JsonConverter<DeviceType>
    {
        public override DeviceType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                var stringValue = reader.GetString();
                if (Enum.TryParse<DeviceType>(stringValue, true, out var deviceType))
                {
                    return deviceType;
                }
            }
            catch (Exception)
            {
                var intValue = reader.GetInt32();
                return (DeviceType)intValue;
            }
            throw new JsonException("Unable to convert to DeviceType.");
        }

        public override void Write(Utf8JsonWriter writer, DeviceType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
