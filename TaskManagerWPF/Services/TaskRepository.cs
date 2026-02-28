using System.Data.SQLite;
using TaskManagerWPF.Models;
using TaskManagerWPF.Data;

namespace TaskManagerWPF.Services;

public class TaskRepository
{
    public List<TaskModel> GetAll()
    {
        List<TaskModel> taskuri = new();

        using var connection = DatabaseHelper.GetConnection();
        string query = "SELECT * FROM Taskuri";

        using var command = new SQLiteCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            DateTime? deadline = null;

            var raw = reader["Deadline"];

            if (raw != DBNull.Value)
            {
                if (DateTime.TryParse(raw.ToString(), out var parsed))
                    deadline = parsed;
            }
            taskuri.Add(new TaskModel
            {
                Id = reader.GetInt32(0),
                Titlu = reader.GetString(1),
                Descriere = reader.IsDBNull(2) ? "" : reader.GetString(2),
                Deadline = deadline,
                Categorie = (CategoriiTask)reader.GetInt32(4),
                Status = (StatusTask)reader.GetInt32(5),
                Prioritate = (PrioritateTask)reader.GetInt32(6)
            });
        }

        return taskuri;
    }

    public int Insert(TaskModel task)
    {
        using var connection = DatabaseHelper.GetConnection();

        string insert = @"INSERT INTO Taskuri
        (Titlu, Descriere, Deadline, Categorie, Status, Prioritate)
        VALUES (@Titlu,@Descriere,@Deadline,@Categorie,@Status,@Prioritate);
        SELECT last_insert_rowid();";

        using var cmd = new SQLiteCommand(insert, connection);

        cmd.Parameters.AddWithValue("@Titlu", task.Titlu);
        cmd.Parameters.AddWithValue("@Descriere", task.Descriere);
        cmd.Parameters.AddWithValue("@Deadline", task.Deadline);
        cmd.Parameters.AddWithValue("@Categorie", (int)task.Categorie);
        cmd.Parameters.AddWithValue("@Status", (int)task.Status);
        cmd.Parameters.AddWithValue("@Prioritate", (int)task.Prioritate);

        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public void Update(TaskModel task)
    {
        using var connection = DatabaseHelper.GetConnection();

        string update = @"UPDATE Taskuri SET
        Titlu=@Titlu,
        Descriere=@Descriere,
        Deadline=@Deadline,
        Categorie=@Categorie,
        Status=@Status,
        Prioritate=@Prioritate
        WHERE Id=@Id";

        using var cmd = new SQLiteCommand(update, connection);

        cmd.Parameters.AddWithValue("@Titlu", task.Titlu);
        cmd.Parameters.AddWithValue("@Descriere", task.Descriere);
        cmd.Parameters.AddWithValue("@Deadline", task.Deadline);
        cmd.Parameters.AddWithValue("@Categorie", (int)task.Categorie);
        cmd.Parameters.AddWithValue("@Status", (int)task.Status);
        cmd.Parameters.AddWithValue("@Prioritate", (int)task.Prioritate);
        cmd.Parameters.AddWithValue("@Id", task.Id);

        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = DatabaseHelper.GetConnection();

        string query = "DELETE FROM Taskuri WHERE Id=@id";
        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);
        command.ExecuteNonQuery();
        
    }
}