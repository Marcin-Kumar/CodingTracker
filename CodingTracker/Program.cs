namespace CodingTracker;

internal class Program
{
    private static void Main(string[] args)
    {
        CodingTrackerController codingTrackerController;
        try
        {
            ConfigurationManager configurationManager = ConfigurationManager.GetConfiguration();
            string connectionString = configurationManager.GetConnectionString();
            CodingTrackerRepository codingTrackerRepository = new(connectionString);
            codingTrackerController = new(codingTrackerRepository);
            codingTrackerController.RunCodingTracker();
        }
        catch (ArgumentNullException ex)
        {
            ConsoleLogger.WriteLineInRed($"Error during configuration: {ex.Message}\n{ex.StackTrace}");
        }
    }
}