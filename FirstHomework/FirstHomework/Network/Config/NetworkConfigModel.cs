using System.Net;
using System.Text.Json.Serialization;

namespace FirstHomework.Network.Config;

public class NetworkConfigModel(string serverHostAsString, string serverPortAsString)
{
    [JsonPropertyName("ServerHost")]
    public string ServerHostAsString { get; } = serverHostAsString;

    public IPAddress Host => IPAddress.Parse(ServerHostAsString);

    [JsonPropertyName("ServerPort")]
    public string ServerPortAsString { get; } = serverPortAsString;

    public int Port => int.Parse(ServerPortAsString);

}
