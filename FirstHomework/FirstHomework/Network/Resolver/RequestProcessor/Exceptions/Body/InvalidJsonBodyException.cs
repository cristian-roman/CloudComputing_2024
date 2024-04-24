using FirstHomework.Network.Sender.Response;

namespace FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Body;

public class InvalidJsonBodyException():
    RequestProcessingException("The body of the request is not a valid JSON object.", new ResponseStatusModel(400));