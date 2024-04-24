using FirstHomework.Network.Sender.Response;

namespace FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Method;

public class UnexpectedRequestMethodException(string requestMethod)
: RequestProcessingException($"Unexpected request method: {requestMethod}\n" +
                             "Expected methods: GET, POST, PUT, PATCH, DELETE",
    new ResponseStatusModel(501));