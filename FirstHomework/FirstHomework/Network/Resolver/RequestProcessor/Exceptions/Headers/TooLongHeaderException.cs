using FirstHomework.Network.Sender.Response;

namespace FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Headers;

public class TooLongHeaderException() : RequestProcessingException
    ("The header is too long. The maximum length is 256 bytes.",
        new ResponseStatusModel(431));