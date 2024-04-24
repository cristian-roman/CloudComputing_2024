using FirstHomework.Network.Sender.Response;

namespace FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Request;

public class UnexpectedRequestContentException(string request)
    : RequestProcessingException("Unexpected request content accordingly to HTTP/1.1 protocol.\n" +
                                 "Expected request:\n" +
                                 "<METHOD> <PATH> <PROTOCOL>\n" +
                                 "<HEADER>: <VALUE> (one or more separated by end line)\n" +
                                 "<BODY> (optional, but if existent must be in json format and Content-Type, Content-Length headers must be in place)\n" +
                                 "Request received:\n" +
                                 request,
        new ResponseStatusModel(400));
