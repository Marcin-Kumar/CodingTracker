namespace CodingTracker;

internal class Program
{
    static void Main(string[] args)
    {
        try
        {
            ConfigurationManager configurationManager = ConfigurationManager.GetConfiguration();
            string connectionString = configurationManager.GetConnectionString();
            CodingTrackerRepository codingTrackerRepository = new(connectionString);
            CodingTrackerController codingTrackerController = new(codingTrackerRepository);
            new CodingTrackerManager(codingTrackerController).RunCodingTracker();
        }
        catch (ArgumentNullException ex) { 
            Console.WriteLine($"Error during configuration: {ex.Message}\n{ex.StackTrace}");
        }
    }
}
