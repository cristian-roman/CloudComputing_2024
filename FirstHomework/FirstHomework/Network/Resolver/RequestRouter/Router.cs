using System.Reflection;
using System.Text.RegularExpressions;
using FirstHomework.APIs;
using FirstHomework.Network.Resolver.RequestProcessor;

namespace FirstHomework.Network.Resolver.RequestRouter;

public static partial class Router
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
        //replace guid from path with {id}
        var regex = GuidRegex();
        path = regex.Replace(path, "{id}");

        regex = DateRegex();
        path = regex.Replace(path, "{timestamp}");

        var answer = Routes.FirstOrDefault
            (route => route.Key.Key == method && route.Key.Value == path);
        if (answer.Value is null)
        {
            throw new KeyNotFoundException();
        }

        return answer.Value;
    }

    [GeneratedRegex(@"\b[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}\b")]
    public static partial Regex GuidRegex();

    [GeneratedRegex(@"\b\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\b")]
    public static partial Regex DateRegex();
}