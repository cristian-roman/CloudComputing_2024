using FirstHomework.Network.Sender.Response;

namespace FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Path;

public class EmptyRequestPathException():RequestProcessingException
    ("The path must not be empty", new ResponseStatusModel(400));