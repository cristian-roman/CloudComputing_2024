using FirstHomework.Network.Sender.Response;

namespace FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Request;

public class PropertyMissingException(string message)
    : RequestProcessingException(message, new ResponseStatusModel(422));