using FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Body.BodyHeaders;
using FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Headers;
using FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Method;
using FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Path;
using FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Protocol;
using FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Request;
using UnexpectedContentTypeException = FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Body.BodyHeaders.UnexpectedContentTypeException;

namespace FirstHomework.Network.Resolver.RequestProcessor;

public static class RequestValidator
{
    public static void ValidateRequest(string request)
    {
        if (request.Length > 8192)
        {
            throw new TooLongRequestException();
        }
    }
    public static void ValidateMethod(string method)
    {
        if (method != "GET" && method != "POST" && method != "PUT" && method != "PATCH" && method != "DELETE")
        {
            throw new UnexpectedRequestMethodException(method);
        }
    }

    public static void ValidatePath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new EmptyRequestPathException();
        }

        if (path[0] != '/')
        {
            throw new UnexpectedRequestPathException(path);
        }

        if (path.Length > 256)
        {
            throw new TooLongPathException();
        }
    }

    public static void ValidateProtocol(string protocol)
    {
        if (protocol != "HTTP/1.1")
        {
            throw new UnexpectedRequestProtocolException(protocol);
        }
    }

    public static void ValidateHeader(string headerLabel, string headerValue)
    {
        var lng = headerLabel.Length + headerValue.Length + 2;
        if (lng > 256)
        {
            throw new TooLongHeaderException();
        }

        switch (headerLabel)
        {
            case "Content-Type" when !headerValue.Contains("application/json"):
                throw new InvalidContentTypeHeaderException(headerValue);
            case "Content-Length" when !int.TryParse(headerValue, out _):
                throw new InvalidContentLengthException(headerValue);
        }
    }

    public static void ValidateBody(Dictionary<string, string> headers, string? body)
    {
        if (string.IsNullOrEmpty(body))
        {
            if (headers.ContainsKey("Content-Type"))
            {
                throw new UnexpectedContentTypeException();
            }

            if (!headers.TryGetValue("Content-Length", out var value)) return;
            if (value != "0")
            {
                throw new UnexpectedContentLengthException();
            }
        }
        else
        {
            if (!headers.ContainsKey("Content-Type"))
            {
                throw new ContentTypeHeaderMissingException();
            }

            if (!headers.TryGetValue("Content-Length", out var value))
            {
                throw new ContentLengthHeaderMissingException();
            }

            if (value != body.Length.ToString())
            {
                throw new InconsistentRequestBodyLengthException(body.Length, int.Parse(value));
            }
        }
    }
}