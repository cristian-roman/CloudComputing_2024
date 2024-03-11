using FirstHomework.Network.Sender.Response;

namespace FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Body;

public class UnexpectedBodyException(string message): RequestProcessingException(message, new ResponseStatusModel(400));
