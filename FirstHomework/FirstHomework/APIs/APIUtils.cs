using System.Text.RegularExpressions;
using FirstHomework.Network.Resolver.RequestProcessor;
using FirstHomework.Network.Resolver.RequestRouter;
namespace FirstHomework.APIs;

public static class ApiUtils
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

    public static Task<List<DateTime>> ExtractDatesFromPath(RequestModel model)
    {
        var path = model.Path;
        var regex = Router.DateRegex();
        var matches = regex.Matches(path);
        var dates = new List<DateTime>();
        foreach (Match match in matches)
        {
            dates.Add(DateTime.Parse(match.Value));
        }
        return Task.FromResult(dates);
    }
}