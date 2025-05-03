using Spectre.Console;

namespace CodingTracker;

internal static class ConsoleLogger
{
    internal static void WriteLineInBoldYellow(string message)
    {
        AnsiConsole.MarkupLine($"[bold yellow]{message}[/]");
    }

    internal static void WriteLineInRed(string message)
    {
        AnsiConsole.MarkupLine($"[red]{message}[/]");
    }
}