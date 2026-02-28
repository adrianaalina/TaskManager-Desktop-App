using System.Data.SQLite;
using System.IO;

namespace TaskManagerWPF.Data;

public static class DatabaseHelper
{
    private static readonly string folder =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

    private static readonly string dbPath =
        Path.Combine(folder, "taskuri.db");

    private static readonly string connectionString =
        $"Data Source={dbPath};Version=3;";

    public static SQLiteConnection GetConnection()
    {
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        if (!File.Exists(dbPath))
            SQLiteConnection.CreateFile(dbPath);

        var connection = new SQLiteConnection(connectionString);
        connection.Open();

        EnsureTables(connection);

        return connection;
    }
    
    private static void EnsureTables(SQLiteConnection connection)
    {
        string createTable = @"
    CREATE TABLE IF NOT EXISTS Taskuri (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Titlu TEXT NOT NULL,
        Descriere TEXT,
        Deadline TEXT,
        Categorie INTEGER,
        Status INTEGER,
        Prioritate INTEGER
    );";

        using var command = new SQLiteCommand(createTable, connection);
        command.ExecuteNonQuery();
    }
}