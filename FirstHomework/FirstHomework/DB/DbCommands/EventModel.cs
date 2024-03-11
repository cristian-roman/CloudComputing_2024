using System.Text.Json.Serialization;

namespace FirstHomework.DB.DbCommands;

public class EventModel
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string EventName { get; set; }

    [JsonPropertyName("begins")]
    public DateTime Begins { get; set; }

    [JsonPropertyName("ends")]
    public DateTime Ends { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}