using FirstHomework.Network.Resolver.RequestProcessor;
using FirstHomework.Network.Resolver.RequestRouter;

namespace FirstHomework.APIs;

public class API
{
    [Route("GET", "/")]
    public static string GetHelloWorld(RequestModel request)
    {
        return "Hello, World! ahfihgiohadghoidshgdshig";
    }
}