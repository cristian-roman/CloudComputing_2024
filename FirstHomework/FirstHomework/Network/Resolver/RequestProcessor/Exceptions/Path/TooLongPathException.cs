using FirstHomework.Network.Sender.Response;

namespace FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Path;

public class TooLongPathException() : RequestProcessingException("The path must not exceed 256 characters in length.",
    new ResponseStatusModel(414));