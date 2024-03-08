using FirstHomework.Network.Sender.Response;

namespace FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Path;

public class UnexpectedRequestPathException(string path):RequestProcessingException
    ($"Unexpected request path: {path}.\n" +
     "The path must begin with a forward slash and must have tokens separated by one and only slash each.\n" +
     "The token format can be anything but empty string.\n" +
     "The path must not exceed 256 characters in length.",
        new ResponseStatusModel(400));