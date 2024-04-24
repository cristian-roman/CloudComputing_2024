using FirstHomework.Network.Sender.Response;

namespace FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Protocol;

public class UnexpectedRequestProtocolException(string protocol):RequestProcessingException
    ($"Unexpected request protocol: {protocol}.\n" +
     "The protocol must be HTTP/1.1.",
        new ResponseStatusModel(505));