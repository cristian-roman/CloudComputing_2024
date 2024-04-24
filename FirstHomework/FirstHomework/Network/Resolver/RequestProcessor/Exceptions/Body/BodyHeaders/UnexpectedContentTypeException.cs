using FirstHomework.Network.Sender.Response;

namespace FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Body.BodyHeaders;

public class UnexpectedContentTypeException()
    : RequestProcessingException("Content-Type header is unexpected while the body is null or empty.\n" +
                                 "Remove it if no body was intended to be sent or add a body to the request.",
        new ResponseStatusModel(400));