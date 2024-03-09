using FirstHomework.Network;
using FirstHomework.Network.Resolver.RequestRouter;

namespace FirstHomework;

public static class Program
{
    private static string GetNetworkConfigFilePath(IReadOnlyList<string> args)
    {
        ArgumentNullException.ThrowIfNull(args);
        if (args.Count == 0)
        {
            throw new ArgumentException("Network config file path is required");
        }

        var path = Path.Combine(args[0], "NetworkConfig.json");
        if (!File.Exists(path))
        {
            throw new ArgumentException("Network config file does not exist");
        }

        return path;
    }

    private static async Task Main(string[] args)
    {
        var networkConfigFilePath = GetNetworkConfigFilePath(args);
        Router.AddRoutes();
        var server = new ServerNetwork(networkConfigFilePath);
        await server.StartServer();
    }
}