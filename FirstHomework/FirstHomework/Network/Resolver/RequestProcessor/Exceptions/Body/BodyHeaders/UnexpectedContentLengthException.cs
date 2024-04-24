using FirstHomework.Network.Sender.Response;

namespace FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Body.BodyHeaders;

public class UnexpectedContentLengthException()
    : RequestProcessingException("Content-Length header must not exist or must be set to 0 when no body was intended.\n" +
                                 "Remove it or set it to 0 if no body was intended to be sent.",
        new ResponseStatusModel(400));