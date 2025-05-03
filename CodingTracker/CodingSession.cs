namespace CodingTracker;

internal record CodingSession(DateTime StartDateTime, DateTime EndDateTime, int? Id = null)
{
    internal TimeSpan Duration => EndDateTime - StartDateTime;
}