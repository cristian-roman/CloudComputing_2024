using System.Text.Json;
using FirstHomework.DB.DbCommands;
using FirstHomework.Network.Resolver.RequestProcessor;
using FirstHomework.Network.Resolver.RequestRouter;
using FirstHomework.Network.Sender.Response;

namespace FirstHomework.APIs;

public class API
{
    [Route("POST", "/events")]
    public static async Task<APIResponse> CreateEvent(RequestModel request)
    {
        if (request.Body == null)
        {
            return new APIResponse("Body is empty", new ResponseStatusModel(400));
        }
        try
        {
            var eventModel = request.Body.Deserialize<EventModel>();
            await Events.CreateEvent(eventModel);
            return new APIResponse("Event created", new ResponseStatusModel(201));
        }
        catch (Exception e)
        {
            return new APIResponse(e.Message, new ResponseStatusModel(500));
        }
    }
}