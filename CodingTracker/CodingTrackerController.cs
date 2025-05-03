using System.Globalization;
using Spectre.Console;

namespace CodingTracker;

internal class CodingTrackerController
{
    private CodingTrackerRepository _codingTrackerRepository;

    public CodingTrackerController(CodingTrackerRepository codingTrackerRepository)
    {
        _codingTrackerRepository = codingTrackerRepository;
    }

    internal void RunCodingTracker()
    {
        bool exitApp = false;
        while (!exitApp)
        {
            ShowMenu();
            char menuOption = GetMenuOption();
            AnsiConsole.WriteLine();
            exitApp = ExecuteCorrectProcess(menuOption);
        }
    }

    private static void ShowCodingSessionDetails(List<CodingSession> sessions)
    {
        Table table = new();
        table.Border(TableBorder.DoubleEdge);
        table.ShowRowSeparators = true;
        table.AddColumns(["ID", "Start Date", "Start Time", "End Time", "Duration"]);
        foreach (CodingSession record in sessions)
        {
            table.AddRow([record.Id?.ToString() ?? "", record.StartDateTime.ToString(CodingTrackerConstants.DateFormat, CultureInfo.InvariantCulture), record.StartDateTime.ToString(CodingTrackerConstants.TimeFormat, CultureInfo.InvariantCulture), record.EndDateTime.ToString(CodingTrackerConstants.TimeFormat, CultureInfo.InvariantCulture), record.Duration.ToString(@"hh\:mm", CultureInfo.InvariantCulture)]);
        }
        table.Title("[bold yellow]Coding Sessions[/]");
        AnsiConsole.Write(table);
    }

    private bool ExecuteCorrectProcess(char menuOption)
    {
        bool exitApp = false;
        try
        {
            switch (menuOption)
            {
                case 'i':
                    ExecuteInsertProcess();
                    break;

                case 'r':
                    ExecuteDeleteProcess();
                    break;

                case 'u':
                    ExecuteUpdateProcess();
                    break;

                case 'v':
                    ExecuteViewingProcess();
                    break;

                case 'e':
                    exitApp = true;
                    break;

                default:
                    ConsoleLogger.WriteLineInRed("Invalid option entered");
                    break;
            }
        }
        catch (Exception ex) when (ex is ArgumentException || ex is InvalidDataException)
        {
            ConsoleLogger.WriteLineInRed($"{ex.Message}\n");
        }

        return exitApp;
    }

    private void ExecuteDeleteProcess()
    {
        int id;
        ConsoleLogger.WriteLineInBoldYellow($"Please enter the Id of the coding session to remove, the Id's can be viewed when viewing the entries");
        string? idEntered = Console.ReadLine();
        if (!int.TryParse(idEntered, out id))
        {
            throw new ArgumentException("The Id entered was in an inconsistent format");
        }
        _codingTrackerRepository.DeleteCodingSession(id);
    }

    private void ExecuteInsertProcess()
    {
        DateOnly date;
        TimeOnly startTime;
        TimeOnly endTime;
        ConsoleLogger.WriteLineInBoldYellow($"Please enter the date for which you would like to track your coding times, please use the format {CodingTrackerConstants.DateFormat}");
        string? dateEntered = Console.ReadLine();
        ConsoleLogger.WriteLineInBoldYellow($"Please enter the time at which you started coding, please use the format {CodingTrackerConstants.TimeFormat}");
        string? startTimeEntered = Console.ReadLine();
        ConsoleLogger.WriteLineInBoldYellow($"Please enter the time at which you ended coding, please use the format {CodingTrackerConstants.TimeFormat}");
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

    private void ExecuteUpdateProcess()
    {
        int id;
        DateOnly date;
        TimeOnly startTime;
        TimeOnly endTime;
        ConsoleLogger.WriteLineInBoldYellow($"Please enter Id of the coding session to update an entry, the Id's can be viewed when viewing the entries");
        string? idEntered = Console.ReadLine();
        ConsoleLogger.WriteLineInBoldYellow($"Please enter the date for which you would like to track your coding times, please use the format {CodingTrackerConstants.DateFormat}");
        string? dateEntered = Console.ReadLine();
        ConsoleLogger.WriteLineInBoldYellow($"Please enter the time at which you started coding, please use the format {CodingTrackerConstants.TimeFormat}");
        string? startTimeEntered = Console.ReadLine();
        ConsoleLogger.WriteLineInBoldYellow($"Please enter the time at which you ended coding, please use the format {CodingTrackerConstants.TimeFormat}");
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

    private void ExecuteViewingProcess()
    {
        Console.Clear();
        List<CodingSession> sessions = _codingTrackerRepository.FindAllCodingSessions();
        if (sessions.Count != 0)
        {
            ShowCodingSessionDetails(sessions);
        }
        else
        {
            ConsoleLogger.WriteLineInRed("No session records found");
        }
        Thread.Sleep(1800);
    }

    private char GetMenuOption() => char.ToLower(Console.ReadKey().KeyChar);

    private void ShowMenu()
    {
        ConsoleLogger.WriteLineInBoldYellow("Hello, Welcome to the Coding Tracker app!");
        ConsoleLogger.WriteLineInBoldYellow("Please choose an option from below\n");
        ConsoleLogger.WriteLineInBoldYellow("i - to insert an entry");
        ConsoleLogger.WriteLineInBoldYellow("r - to remove an entry");
        ConsoleLogger.WriteLineInBoldYellow("u - to update an entry");
        ConsoleLogger.WriteLineInBoldYellow("v - to view entries");
        ConsoleLogger.WriteLineInBoldYellow("e - to exit\n");
        ConsoleLogger.WriteLineInBoldYellow("Your option: ");
    }
}