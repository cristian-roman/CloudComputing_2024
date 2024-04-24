using FirstHomework.Network.Sender.Exceptions;

namespace FirstHomework.Network.Sender.Response;

public class ResponseStatusModel
{
    public int StatusCode { get; }
    public string StatusMessage { get; }

    public ResponseStatusModel(int statusCode)
    {
        switch(statusCode)
        {
            case 200:
                StatusCode = 200;
                StatusMessage = "OK";
                break;

            case 201:
                StatusCode = 201;
                StatusMessage = "Created";
                break;

            case 400:
                StatusCode = 400;
                StatusMessage = "Bad Request";
                break;

            case 404:
                StatusCode = 404;
                StatusMessage = "Not Found";
                break;

            case 405:
                StatusCode = 405;
                StatusMessage = "Method Not Allowed";
                break;

            case 411:
                StatusCode = 411;
                StatusMessage = "Length Required";
                break;

            case 413:
                StatusCode = 413;
                StatusMessage = "Payload Too Large";
                break;

            case 414:
                StatusCode = 414;
                StatusMessage = "Request-URI Too Long";
                break;

            case 415:
                StatusCode = 415;
                StatusMessage = "Unsupported Media Type";
                break;

            case 422:
                StatusCode = 422;
                StatusMessage = "Unprocessable Entity";
                break;

            case 431:
                StatusCode = 431;
                StatusMessage = "Request Header Fields Too Large";
                break;

            case 500:
                StatusCode = 500;
                StatusMessage = "Internal Server Error";
                break;

            case 501:
                StatusCode = 501;
                StatusMessage = "Not Implemented";
                break;

            case 505:
                StatusCode = 505;
                StatusMessage = "HTTP Version Not Supported";
                break;

            default:
                throw new UnsupportedStatusCodeException(statusCode);
        }
    }
}