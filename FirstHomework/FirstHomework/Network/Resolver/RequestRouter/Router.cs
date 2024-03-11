using System.Reflection;
using FirstHomework.APIs;
using FirstHomework.Network.Resolver.RequestProcessor;

namespace FirstHomework.Network.Resolver.RequestRouter;

public static class Router
{
    private static readonly Dictionary<KeyValuePair<string, string>, Func<RequestModel, Task<APIResponse>>> Routes = new();

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
                    (Func<RequestModel,Task<APIResponse>>) method.CreateDelegate(typeof(Func<RequestModel, Task<APIResponse>>)));
            }
        }
    }

    public static Func<RequestModel, Task<APIResponse>> GetRoute(string method, string path)
    {
        return Routes[new KeyValuePair<string, string>(method, path)];
    }
}