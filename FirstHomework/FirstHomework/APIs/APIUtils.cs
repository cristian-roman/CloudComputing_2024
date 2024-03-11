using System.Text.RegularExpressions;
using FirstHomework.Network.Resolver.RequestProcessor;
using FirstHomework.Network.Resolver.RequestRouter;
namespace FirstHomework.APIs;

public static class APIUtils
{
    public static Task<List<string>> ExtractIdsFromPath(RequestModel model)
    {
        var path = model.Path;
        var regex = Router.GuidRegex();
        var matches = regex.Matches(path);
        var ids = new List<string>();
        foreach (Match match in matches)
        {
            ids.Add(match.Value);
        }
        return Task.FromResult(ids);
    }
}