using Microsoft.Extensions.Configuration;

namespace CodingTracker;

internal class ConfigurationManager
{
    private static ConfigurationManager? s_instance;

    private IConfigurationRoot _configurationRoot;

    private ConfigurationManager()
    {
        _configurationRoot = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
    }

    public static ConfigurationManager GetConfiguration()
    {
        s_instance ??= new ConfigurationManager();
        return s_instance;
    }

    public string GetConnectionString()
    {
        string? connectionString = _configurationRoot.GetConnectionString("DefaultConnection");
        if (connectionString == null)
        {
            throw new ArgumentNullException("SQLite connection string isn't configured");
        }
        return connectionString;
    }
}