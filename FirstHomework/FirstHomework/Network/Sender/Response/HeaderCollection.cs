using System.Globalization;
using System.Text.Json;

namespace FirstHomework.Network.Sender.Response;

public class HeaderCollection(string? message)
{
    public Dictionary<string, string> Dict { get; } = new()
    {
        { "Content-Type", IsJson(message) ? "application/json" : "text/plain" },
        { "Content-Length", message.Length.ToString() },
        { "Date", DateTime.Now.ToString(CultureInfo.CurrentCulture)}
    };

    private static bool IsJson(string? message)
    {
        try
        {
            JsonDocument.Parse(message);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}