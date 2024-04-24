using FirstHomework.Network.Sender.Response;

namespace FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Request;

public class TooLongRequestException() : RequestProcessingException(
    "The request is too long. The maximum length is 8192 bytes.",
    new ResponseStatusModel(413));