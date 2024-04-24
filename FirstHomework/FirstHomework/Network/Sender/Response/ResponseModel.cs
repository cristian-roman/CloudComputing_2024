using System.Text;

namespace FirstHomework.Network.Sender.Response;

public class ResponseModel(ResponseStatusModel responseStatus, string message, HeaderCollection headers)
{
    private const string Protocol = "HTTP/1.1";
    private ResponseStatusModel ResponseStatus { get; } = responseStatus;
    private string Message { get; } = message;
    public HeaderCollection Headers { get; } = headers;

    public byte[] ToBytes()
    {
        var responseLine = $"{Protocol} {ResponseStatus.StatusCode} {ResponseStatus.StatusMessage}";
        var headers = string.Join('\n', Headers.Dict.Select(header => $"{header.Key}: {header.Value}"));
        return Encoding.UTF8.GetBytes($"{responseLine}\n{headers}\n\n{Message}");
    }

    public string ToString()
    {
        var responseLine = $"{Protocol} {ResponseStatus.StatusCode} {ResponseStatus.StatusMessage}";
        var headers = string.Join('\n', Headers.Dict.Select(header => $"{header.Key}: {header.Value}"));
        return $"{responseLine}\n{headers}\n\n{Message}";
    }
}