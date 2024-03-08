using FirstHomework.Network.Sender.Response;

namespace FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Headers;

public class InvalidHeaderException(string header) :
    RequestProcessingException("The header received does not respect the format: \n" +
                               "Header: Value\n" +
                               "Received: " + header,
        new ResponseStatusModel(400));