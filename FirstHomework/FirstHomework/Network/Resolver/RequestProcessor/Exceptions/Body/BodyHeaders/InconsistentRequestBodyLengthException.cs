using FirstHomework.Network.Sender.Response;

namespace FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Body.BodyHeaders;

public class InconsistentRequestBodyLengthException(int bodyLength, int contentLength)
    : RequestProcessingException($"The request body length is inconsistent with the Content-Length header.\n" +
                                 $"The body length is {bodyLength} bytes, " +
                                 $"but the Content-Length header specifies {contentLength} bytes.",
        new ResponseStatusModel(411)  );