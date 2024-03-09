using System.Reflection;
using FirstHomework.Network.Resolver.RequestProcessor;

namespace FirstHomework.Network.Resolver.RequestRouter;

public static class Router
{
    private static readonly Dictionary<KeyValuePair<string, string>, Func<RequestModel, string>> Routes = new();

    public static void AddRoutes()
    {
        var methods = Assembly.GetExecutingAssembly().GetTypes()
            .SelectMany(t => t.GetMethods())
            .Where(m => m.GetCustomAttributes(typeof(RouteAttribute), false).Length > 0)
            .ToArray();

        foreach (var method in methods)
        {
            var routeAttribute = method.GetCustomAttribute<RouteAttribute>();
            if (routeAttribute is not null)
            {
                Routes.Add(new KeyValuePair<string, string>(routeAttribute.Method, routeAttribute.Path),
                    (Func<RequestModel, string>) method.CreateDelegate(typeof(Func<RequestModel, string>)));
            }
        }
    }

    public static Func<RequestModel, string> GetRoute(string method, string path)
    {
        return Routes[new KeyValuePair<string, string>(method, path)];
    }
}