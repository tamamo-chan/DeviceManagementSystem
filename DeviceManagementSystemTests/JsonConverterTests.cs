using DeviceManagementSystem.Core.Entities;
using System.Text;
using System.Text.Json;

namespace DeviceManagementSystemTests
{
    public class JsonConverterTests
    {
        [Theory]
        [InlineData("{\"SerialNumber\":\"d9bfb5ff-89d4-4e8c-bb3d-51d1d79e3b8f\", \"ModelId\":\"A123\",\"ModelName\":\"Latitude 5490\",\"Manufacturer\":\"Dell\",\"PrimaryUser\":\"null\", \"OperatingSystem\":\"Windows 11\",\"DeviceType\":0,\"Status\":0}", true)]
        [InlineData("{\"SerialNumber\":\"d9bfb5ff-89d4-4e8c-bb3d-51d1d79e3b8f\", \"ModelId\":\"A123\",\"ModelName\":\"Latitude 5490\",\"Manufacturer\":\"Dell\",\"PrimaryUser\":\"null\", \"OperatingSystem\":\"Windows 11\",\"DeviceType\":\"Laptop\",\"Status\":0}", true)]
        [InlineData("{\"SerialNumber\":\"d9bfb5ff-89d4-4e8c-bb3d-51d1d79e3b8f\", \"ModelId\":\"A123\",\"ModelName\":\"Latitude 5490\",\"Manufacturer\":\"Dell\",\"PrimaryUser\":\"null\", \"OperatingSystem\":\"Windows 11\",\"DeviceType\":\"laptop\",\"Status\":0}", true)]
        [InlineData("{\"SerialNumber\":\"d9bfb5ff-89d4-4e8c-bb3d-51d1d79e3b8f\", \"ModelId\":\"A123\",\"ModelName\":\"Latitude 5490\",\"Manufacturer\":\"Dell\",\"PrimaryUser\":\"null\", \"OperatingSystem\":\"Windows 11\",\"DeviceType\":\"test\",\"Status\":0}", false)]
        [InlineData("{\"SerialNumber\":\"d9bfb5ff-89d4-4e8c-bb3d-51d1d79e3b8f\", \"ModelId\":\"A123\",\"ModelName\":\"Latitude 5490\",\"Manufacturer\":\"Dell\",\"PrimaryUser\":\"null\", \"OperatingSystem\":\"Windows 11\",\"DeviceType\":0,\"Status\":\"Active\"}", true)]
        [InlineData("{\"SerialNumber\":\"d9bfb5ff-89d4-4e8c-bb3d-51d1d79e3b8f\", \"ModelId\":\"A123\",\"ModelName\":\"Latitude 5490\",\"Manufacturer\":\"Dell\",\"PrimaryUser\":\"null\", \"OperatingSystem\":\"Windows 11\",\"DeviceType\":0,\"Status\":\"active\"}", true)]
        [InlineData("{\"SerialNumber\":\"d9bfb5ff-89d4-4e8c-bb3d-51d1d79e3b8f\", \"ModelId\":\"A123\",\"ModelName\":\"Latitude 5490\",\"Manufacturer\":\"Dell\",\"PrimaryUser\":\"null\", \"OperatingSystem\":\"Windows 11\",\"DeviceType\":0,\"Status\":\"test\"}", false)]
        public void AddDevice_ShouldAddDevice(string jsonInput, bool expectedSuccess)
        {
            // Arrange
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonInput ?? ""));
            string requestBody = new StreamReader(stream).ReadToEnd();
            Device? device = null;

            // Act + Assert
            try
            {
                device = JsonSerializer.Deserialize<Device>(requestBody);
                if (!expectedSuccess) { Assert.Fail(""); }
            }
            catch (Exception)
            {
                if (expectedSuccess) { Assert.Fail(""); }
            }
        }
    }
}
