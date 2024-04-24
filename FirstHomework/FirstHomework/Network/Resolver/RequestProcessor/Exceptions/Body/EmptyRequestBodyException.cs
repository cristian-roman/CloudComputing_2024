using FirstHomework.Network.Sender.Response;

namespace FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Body;

public class EmptyRequestBodyException(string message)
    : RequestProcessingException(message, new ResponseStatusModel(422));