using FirstHomework.DB.Config;
using Npgsql;

namespace FirstHomework.DB.DbCommands;

public static class Events
{
    public static async Task CreateEvent(EventModel eventModel)
    {
        DbLoader.Connection!.Open();
        const string query = "INSERT INTO evenimente (name, begins, ends, description) VALUES (@name, @begins, @ends, @description)";
        await using var cmd = new NpgsqlCommand(query, DbLoader.Connection);
        cmd.Parameters.AddWithValue("name", eventModel.EventName);
        cmd.Parameters.AddWithValue("begins", eventModel.Begins);
        cmd.Parameters.AddWithValue("ends", eventModel.Ends);
        cmd.Parameters.AddWithValue("description", eventModel.Description);
        await Task.Run(()=>cmd.ExecuteNonQuery());
        await DbLoader.Connection.CloseAsync();
    }
}