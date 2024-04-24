using FirstHomework.Network.Sender.Response;

namespace FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Body.BodyHeaders;

public class ContentLengthHeaderMissingException() :
    RequestProcessingException("The Content-Length header is missing. Add a valid one and try again.",
    new ResponseStatusModel(411));