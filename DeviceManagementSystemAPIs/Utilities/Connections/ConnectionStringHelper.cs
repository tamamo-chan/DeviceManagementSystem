using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace DeviceManagementSystem.Utilities.Connections
{
    public class ConnectionStringHelper
    {
        public static string? GetCustomConnectionString(string name)
        {
            string? conStr = System.Environment.GetEnvironmentVariable($"ConnectionStrings:{name}", EnvironmentVariableTarget.Process);
            if (string.IsNullOrEmpty(conStr)) // Azure Functions App Service naming convention
                conStr = System.Environment.GetEnvironmentVariable($"CUSTOMCONNSTR_{name}", EnvironmentVariableTarget.Process);
            if (string.IsNullOrEmpty(conStr))
            {
                ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
                IConfiguration configuration = configurationBuilder.AddUserSecrets<Program>().Build();
                conStr = configuration.GetValue<string>($"ConnectionStrings:{name}");
            }
            return conStr;
        }
    }
}
