using System.Text.Json.Serialization;

namespace FirstHomework.DB.Config;

public class DbModel
{
    [JsonPropertyName("Host")]
    public string Host { get; set; }

    [JsonPropertyName("Port")]
    public string Port { get; set; }

    [JsonPropertyName("DbName")]
    public string DbName { get; set; }

    [JsonPropertyName("User")]
    public string User { get; set; }

    [JsonPropertyName("Password")]
    public string Password { get; set; }
}