using FirstHomework.DB.Config;
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

    private static string GetDatabaseConfigFilePath(IReadOnlyList<string> args)
    {
        ArgumentNullException.ThrowIfNull(args);
        if (args.Count == 1)
        {
            throw new ArgumentException("Database config file path is required");
        }

        var path = Path.Combine(args[1], "DbConfig.json");
        if (!File.Exists(path))
        {
            throw new ArgumentException("Database config file does not exist");
        }

        return path;
    }

    private static async Task Main(string[] args)
    {
        var networkConfigFilePath = GetNetworkConfigFilePath(args);
        Router.AddRoutes();
        DbLoader.LoadConnection(GetDatabaseConfigFilePath(args));
        var server = new ServerNetwork(networkConfigFilePath);
        await server.StartServer();
    }
}