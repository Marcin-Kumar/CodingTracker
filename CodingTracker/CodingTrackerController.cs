using System.Globalization;

namespace CodingTracker;

internal class CodingTrackerController
{
    private CodingTrackerRepository _codingTrackerRepository;

    public CodingTrackerController(CodingTrackerRepository codingTrackerRepository)
    {
        _codingTrackerRepository = codingTrackerRepository;
    }

    internal void ExecuteDeleteProcess()
    {
        throw new NotImplementedException();
    }

    internal void ExecuteInsertProcess()
    {
        DateOnly date;
        TimeOnly startTime;
        TimeOnly endTime;
        Console.WriteLine($"Please enter the date for which you would like to track your coding times, please use the format {CodingTrackerConstants.DateFormat}");
        string? dateEntered = Console.ReadLine();
        Console.WriteLine($"Please enter the time at which you started coding, please use the format {CodingTrackerConstants.TimeFormat}");
        string? startTimeEntered = Console.ReadLine();
        Console.WriteLine($"Please enter the time at which you ended coding, please use the format {CodingTrackerConstants.TimeFormat}");
        string? endTimeEntered = Console.ReadLine();
        bool isDateEnteredInCorrectFormat = DateOnly.TryParseExact(dateEntered, CodingTrackerConstants.DateFormat, CultureInfo.InvariantCulture,
    DateTimeStyles.None, out date);
        bool isStartTimeEnteredInCorrectFormat = TimeOnly.TryParseExact(startTimeEntered, CodingTrackerConstants.TimeFormat, CultureInfo.InvariantCulture,
    DateTimeStyles.None, out startTime);
        bool isEndTimeEnteredInCorrectFormat = TimeOnly.TryParseExact(endTimeEntered, CodingTrackerConstants.TimeFormat, CultureInfo.InvariantCulture,
    DateTimeStyles.None, out endTime);
        if (!isDateEnteredInCorrectFormat || !isStartTimeEnteredInCorrectFormat || !isEndTimeEnteredInCorrectFormat)
        {
            throw new ArgumentException("Either the date, start time or end time entered was in an inconsistent format");
        }

        if (startTime > endTime)
        {
            throw new ArgumentException("Start time shouldn't be after end time");
        }
        _codingTrackerRepository.InsertCodingSession(new CodingSession(StartDateTime: new DateTime(date, startTime), EndDateTime: new DateTime(date, endTime)));
    }

    internal void ExecuteUpdateProcess()
    {
        int id;
        DateOnly date;
        TimeOnly startTime;
        TimeOnly endTime;
        Console.WriteLine($"Please enter Id of the coding session to update an entry, the Id's can be viewed when viewing the entries");
        string? idEntered = Console.ReadLine();
        Console.WriteLine($"Please enter the date for which you would like to track your coding times, please use the format {CodingTrackerConstants.DateFormat}");
        string? dateEntered = Console.ReadLine();
        Console.WriteLine($"Please enter the time at which you started coding, please use the format {CodingTrackerConstants.TimeFormat}");
        string? startTimeEntered = Console.ReadLine();
        Console.WriteLine($"Please enter the time at which you ended coding, please use the format {CodingTrackerConstants.TimeFormat}");
        string? endTimeEntered = Console.ReadLine();
        bool isIdEnteredInCorrectFormat = int.TryParse(idEntered, out id);
        bool isDateEnteredInCorrectFormat = DateOnly.TryParseExact(dateEntered, CodingTrackerConstants.DateFormat, CultureInfo.InvariantCulture,
    DateTimeStyles.None, out date);
        bool isStartTimeEnteredInCorrectFormat = TimeOnly.TryParseExact(startTimeEntered, CodingTrackerConstants.TimeFormat, CultureInfo.InvariantCulture,
    DateTimeStyles.None, out startTime);
        bool isEndTimeEnteredInCorrectFormat = TimeOnly.TryParseExact(endTimeEntered, CodingTrackerConstants.TimeFormat, CultureInfo.InvariantCulture,
    DateTimeStyles.None, out endTime);
        if (!isIdEnteredInCorrectFormat || !isDateEnteredInCorrectFormat || !isStartTimeEnteredInCorrectFormat || !isEndTimeEnteredInCorrectFormat)
        {
            throw new ArgumentException("Either the Id, date, start time or end time entered was in an inconsistent format");
        }

        if (startTime > endTime)
        {
            throw new ArgumentException("Start time shouldn't be after end time");
        }

        _codingTrackerRepository.UpdateCodingSession(new CodingSession(new DateTime(date, startTime), new DateTime(date, endTime), id));
    }

    internal void ExecuteViewingProcess()
    {
        Console.Clear();
        List<CodingSession> sessions = _codingTrackerRepository.FindAllCodingSessions();
        if (sessions.Count != 0)
        {
            Console.WriteLine("Coding Session\n\nId\tStart Date\t\tStart Time\t\tEnd Time\t\tDuration");
            foreach (CodingSession record in sessions)
            {
                Console.WriteLine($"{record.Id}\t{record.StartDateTime.ToString(CodingTrackerConstants.DateFormat, CultureInfo.InvariantCulture)}\t\t{record.StartDateTime.ToString(CodingTrackerConstants.TimeFormat, CultureInfo.InvariantCulture)}\t\t\t{record.EndDateTime.ToString(CodingTrackerConstants.TimeFormat, CultureInfo.InvariantCulture)}\t\t\t{record.Duration.ToString(@"hh\:mm", CultureInfo.InvariantCulture)}");
            }
        }
        else
        {
            Console.WriteLine("No session records found");
        }
        Thread.Sleep(1800);
    }

    internal char GetMenuOption() => char.ToLower(Console.ReadKey().KeyChar);

    internal void ShowMenu()
    {
        Console.WriteLine("Hello, Welcome to the Coding Tracker app!");
        Console.WriteLine("Please choose an option from below\n");
        Console.WriteLine("i - to insert an entry");
        Console.WriteLine("r - to remove an entry");
        Console.WriteLine("u - to update an entry");
        Console.WriteLine("v - to view entries");
        Console.WriteLine("e - to exit\n");
        Console.WriteLine("Your option: ");
    }
}