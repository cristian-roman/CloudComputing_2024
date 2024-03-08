using System.Text.Json;
using FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Body;
using FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Headers;
using FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Request;

namespace FirstHomework.Network.Resolver.RequestProcessor;

public class RequestModel
{
    public string Method { get; private set; } = null!;
    public string Path { get; private set; } = null!;
    public string Protocol { get; private set; } = null!;
    public Dictionary<string, string> Headers { get; private set; } = null!;
    public JsonDocument? Body { get; private set; }

    public RequestModel(string request)
    {
        RequestValidator.ValidateRequest(request);

        FillMethodPathAndProtocol(request);
        ExtractHeaders(request);

        var body = ExtractBody(request);
        RequestValidator.ValidateBody(Headers, body);

        if (body == null) return;
        try
        {
            Body = JsonDocument.Parse(body);
        }
        catch (JsonException)
        {
            throw new InvalidJsonBodyException();
        }
    }

    private void FillMethodPathAndProtocol(string request)
    {
        var firstLineIndex = request.IndexOf('\n');

        if (firstLineIndex == -1)
        {
            throw new UnexpectedRequestContentException(request);
        }

        var firstLine = request[..firstLineIndex];
        if  (firstLine[^1] == '\r')
        {
            firstLine = firstLine[..^1];
        }

        var firstLineComponents = firstLine.Split(' ');

        if (firstLineComponents.Length != 3)
        {
            throw new UnexpectedRequestContentException(request);
        }

        Method = firstLineComponents[0];
        Method = Method.Trim();
        RequestValidator.ValidateMethod(Method);

        Path = firstLineComponents[1];
        Path = Path.Trim();
        RequestValidator.ValidatePath(Path);

        Protocol = firstLineComponents[2];
        Protocol = Protocol.Trim();
        RequestValidator.ValidateProtocol(Protocol);
    }

    private void ExtractHeaders(string request)
    {
        var firstLineIndex = request.IndexOf('\n');
        var bodyBeginIndex = request.IndexOf('{');

        if (bodyBeginIndex == -1)
        {
            bodyBeginIndex = request.Length;
        }

        var headersAsString = request[firstLineIndex..bodyBeginIndex];
        Headers = ParseHeaders(headersAsString);
    }

    private static Dictionary<string, string> ParseHeaders(string headers)
    {
        var headerLines = headers.Split("\n");

        var answer = new Dictionary<string, string>();

        foreach (var headerLine in headerLines)
        {
            if (headerLine.Length == 0 || headerLine == "\r")
                continue;

            var separatorIndex = headerLine.IndexOf(": ", StringComparison.Ordinal);

            var step = 1;
            if (separatorIndex == -1)
            {
                separatorIndex = headerLine.IndexOf(':');
                if (separatorIndex == -1)
                {
                    throw new InvalidHeaderException(headerLine);
                }

                step = 2;
            }

            var header = headerLine[..separatorIndex];
            var value = headerLine[(separatorIndex + step)..];

            header = header.Trim();
            value = value.Trim();

            if (value[^1] == '\r')
            {
                value = value[..^1];
            }

            RequestValidator.ValidateHeader(header, value);

            answer[header] = value;
        }

        return answer;
    }

    private static string? ExtractBody(string request)
    {
        var bodyBeginIndex = request.IndexOf('{');

        return bodyBeginIndex == -1 ? null : request[bodyBeginIndex..].Trim();
    }
}