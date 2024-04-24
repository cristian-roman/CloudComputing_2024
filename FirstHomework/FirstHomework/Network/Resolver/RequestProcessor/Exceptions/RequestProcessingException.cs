using FirstHomework.Network.Sender.Response;

namespace FirstHomework.Network.Resolver.RequestProcessor.Exceptions;

public class RequestProcessingException(string message, ResponseStatusModel status) : Exception(message)
{
    public ResponseStatusModel Status { get; } = status;
}