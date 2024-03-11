using FirstHomework.Network.Resolver.RequestProcessor.Exceptions;
using FirstHomework.Network.Sender.Response;

namespace FirstHomework.DB.Exceptions;

public class ResourceNotFoundException(string message)
    : RequestProcessingException(message, new ResponseStatusModel(404));