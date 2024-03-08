using FirstHomework.Network.Sender.Response;

namespace FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Headers;

public class InvalidContentTypeHeaderException(string contentType):
    RequestProcessingException($"Unexpected content type: {contentType}. The content type must be application/json.",
        new ResponseStatusModel(415));