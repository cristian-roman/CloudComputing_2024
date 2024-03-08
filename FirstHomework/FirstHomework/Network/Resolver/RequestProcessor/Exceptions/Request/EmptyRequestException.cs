using FirstHomework.Network.Sender.Response;

namespace FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Request;

public class EmptyRequestException()
    : RequestProcessingException("HTTP protocol requires request data in specific format.", new ResponseStatusModel(400));