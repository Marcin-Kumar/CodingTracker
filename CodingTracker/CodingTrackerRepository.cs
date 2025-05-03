using System.Data.SQLite;
using Dapper;

namespace CodingTracker;

internal class CodingTrackerRepository
{
    private string _connectionString;

    public CodingTrackerRepository(string connectionString)
    {
        _connectionString = connectionString;
        CreateTableIfNotExists();
    }

    public List<CodingSession> FindAllCodingSessions()
    {
        List<CodingSession> codingSessions = new();
        using SQLiteConnection connection = CreateOpenSQLiteConnection();
        string sql = "SELECT coding_sessions_id AS Id, coding_sessions_start_date_time AS StartDateTime,  coding_sessions_end_date_time AS EndDateTime FROM coding_sessions;";
        foreach (var record in connection.Query(sql))
        {
            codingSessions.Add(new CodingSession(
                Id: Convert.ToInt32(record.Id),
                StartDateTime: DateTime.Parse(record.StartDateTime),
                EndDateTime: DateTime.Parse(record.EndDateTime)
             ));
        }
        return codingSessions;
    }

    public void InsertCodingSession(CodingSession codingSession)
    {
        try
        {
            using SQLiteConnection connection = CreateOpenSQLiteConnection();
            string sql = "INSERT INTO coding_sessions (coding_sessions_start_date_time, coding_sessions_end_date_time) VALUES (@StartDateTime, @EndDateTime);";
            if (connection.Execute(sql, codingSession) <= 0)
            {
                throw new InvalidDataException("Unable to insert data");
            }
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
        }
    }

    internal void DeleteCodingSession(int id)
    {
        try
        {
            using SQLiteConnection connection = CreateOpenSQLiteConnection();
            string sql = "DELETE FROM coding_sessions WHERE coding_sessions_id = @id;";
            if (connection.Execute(sql, new { id }) <= 0)
            {
                throw new InvalidDataException();
            }
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
        }
    }

    internal void UpdateCodingSession(CodingSession codingSession)
    {
        try
        {
            using SQLiteConnection connection = CreateOpenSQLiteConnection();
            string sql = "UPDATE coding_sessions SET coding_sessions_start_date_time = @StartDateTime, coding_sessions_end_date_time = @EndDateTime WHERE coding_sessions_id = @Id;";
            if (connection.Execute(sql, codingSession) <= 0)
            {
                throw new InvalidDataException("Unable to update data");
            }
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
        }
    }

    private SQLiteConnection CreateOpenSQLiteConnection()
    {
        SQLiteConnection connection = new(_connectionString);
        connection.Open();
        return connection;
    }

    private void CreateTableIfNotExists()
    {
        try
        {
            using SQLiteConnection connection = CreateOpenSQLiteConnection();
            string sql = @"CREATE TABLE IF NOT EXISTS coding_sessions (
                        coding_sessions_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        coding_sessions_start_date_time TEXT,
                        coding_sessions_end_date_time TEXT
                    );";
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
        }
    }
}