using Microsoft.Extensions.Configuration;

namespace CodingTracker;

internal class ConfigurationManager
{
    private static ConfigurationManager? instance;

    private IConfigurationRoot configurationRoot;

    private ConfigurationManager() { 
        configurationRoot = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();     
    }

    public ConfigurationManager GetConfiguration()
    {
        instance ??= new ConfigurationManager();
        return instance;
    }

    public string GetConnectionString()
    {
        string? connectionString = configurationRoot.GetConnectionString("DefaultConnection");
        if (connectionString == null) {
            throw new ArgumentNullException("SQLite connection string isn't configured");
        }
        return connectionString;
    }

}
