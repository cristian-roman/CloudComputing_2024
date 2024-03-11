using FirstHomework.DB.Config;
using FirstHomework.DB.Exceptions;
using Npgsql;

namespace FirstHomework.DB.DbCommands;

public static class Events
{
    public static async Task CreateEvent(EventModel eventModel)
    {
        var query = eventModel.Description switch
        {
            null => "INSERT INTO evenimente (name, begins, ends) VALUES (@name, @begins, @ends)",
            _ => "INSERT INTO evenimente (name, begins, ends, description) VALUES (@name, @begins, @ends, @description)"
        };

        DbLoader.Connection!.Open();
        await using var cmd = new NpgsqlCommand(query, DbLoader.Connection);
        cmd.Parameters.AddWithValue("name", eventModel.EventName!);
        cmd.Parameters.AddWithValue("begins", eventModel.Begins!);
        cmd.Parameters.AddWithValue("ends", eventModel.Ends!);
        if (eventModel.Description != null)
            cmd.Parameters.AddWithValue("description", eventModel.Description);

        await Task.Run(()=>cmd.ExecuteNonQuery());
        await DbLoader.Connection.CloseAsync();
    }

    public static async Task<object> GetEvents()
    {
        DbLoader.Connection!.Open();
        await using var cmd = new NpgsqlCommand("SELECT * FROM evenimente", DbLoader.Connection);
        await using var reader = await cmd.ExecuteReaderAsync();
        var events = new List<EventModel>();
        while (await reader.ReadAsync())
        {
            events.Add(new EventModel
            {
                Id = reader.GetGuid(0),
                EventName = reader.GetString(1),
                Begins = reader.GetDateTime(2),
                Ends = reader.GetDateTime(3),
                Description = reader.IsDBNull(4) ? null : reader.GetString(4)
            });
        }
        await DbLoader.Connection.CloseAsync();
        return events;
    }

    public static async Task ModifyEvent(EventModel eventModel)
    {
        var query = eventModel.Description switch
        {
            null => "UPDATE evenimente SET name = @name, begins = @begins, ends = @ends WHERE id = @id",
            _ => "UPDATE evenimente SET name = @name, begins = @begins, ends = @ends, description = @description WHERE id = @id"
        };

        DbLoader.Connection!.Open();
        await using var cmd = new NpgsqlCommand(query, DbLoader.Connection);
        cmd.Parameters.AddWithValue("id", eventModel.Id!);
        cmd.Parameters.AddWithValue("name", eventModel.EventName!);
        cmd.Parameters.AddWithValue("begins", eventModel.Begins!);
        cmd.Parameters.AddWithValue("ends", eventModel.Ends!);
        cmd.Parameters.AddWithValue("description", eventModel.Description!);

        var rowsAffected = await Task.Run(()=>cmd.ExecuteNonQuery());
        await DbLoader.Connection.CloseAsync();

        if (rowsAffected == 0)
        {
            throw new ResourceNotFoundException("No event with the given id was found.");
        }
    }

    public static async Task DeleteEvent(Guid guid)
    {
        DbLoader.Connection!.Open();
        await using var cmd = new NpgsqlCommand("DELETE FROM evenimente WHERE id = @id", DbLoader.Connection);
        cmd.Parameters.AddWithValue("id", guid);
        var rowsAffected = await Task.Run(()=>cmd.ExecuteNonQuery());
        await DbLoader.Connection.CloseAsync();

        if (rowsAffected == 0)
        {
            throw new ResourceNotFoundException("No event with the given id was found.");
        }
    }
}