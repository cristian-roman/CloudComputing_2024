using FirstHomework.Network.Sender.Response;

namespace FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Body.BodyHeaders;

public class ContentTypeHeaderMissingException()
    : RequestProcessingException("Content-Type header is missing while the body is not null or empty.\n" +
                                 "Add it to the request or remove the body if no Content-Type was intended to be sent.",
        new ResponseStatusModel(400));