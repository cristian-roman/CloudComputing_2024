using FirstHomework.Network.Sender.Response;

namespace FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Headers;

public class InvalidContentLengthException(string contentLength) :
    RequestProcessingException("The value passed in the Content-Length header is not a valid number." +
                               "It must be a positive integer. The value passed was: " + contentLength + ".",
        new ResponseStatusModel(400));