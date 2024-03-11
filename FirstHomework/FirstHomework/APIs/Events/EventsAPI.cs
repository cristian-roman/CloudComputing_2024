using System.Text.Json;
using FirstHomework.DB.DbCommands;
using FirstHomework.Network.Resolver.RequestProcessor;
using FirstHomework.Network.Resolver.RequestProcessor.Exceptions;
using FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Body;
using FirstHomework.Network.Resolver.RequestRouter;
using FirstHomework.Network.Sender.Response;

namespace FirstHomework.APIs.Events;

public class EventsAPI
{
    [Route("POST", "/events")]
    public static async Task<APIResponse> CreateEvent(RequestModel request)
    {
        EventsRequestValidator.CheckNotNullBodyRequirement(request);

        EventModel? eventModel;
        try
        {
            eventModel = request.Body!.Deserialize<EventModel>();
        }
        catch (JsonException)
        {
            throw new UnexpectedBodyException
            ("Request body does not have the proper data types types:\n" +
             "\"name\" -> string\n" +
             "\"begins\" -> the ISO 8601 combined date-time format\n" +
             "\"ends\" -> the ISO 8601 combined date-time format\n" +
             "\"description\" -> optional string\n");
        }

        EventsRequestValidator.CheckRequiredFields(eventModel);
        await DB.DbCommands.Events.CreateEvent(eventModel!);

        if (!EventsRequestValidator.MoreDataThenRequiredDetected(request.Body!))
            return new APIResponse("Event created successfully.", new ResponseStatusModel(201));

        string warningMessage;
        if (eventModel!.Description == null)
        {
            warningMessage = "Description field not detected but other fields were present. " +
                             "If description was intended, please use the patch method to add it.";
        }
        else
        {
            warningMessage = "Extra fields were detected, but they were ignored by the server";
        }

        return new APIResponse( "Event created successfully.\n" + warningMessage, new ResponseStatusModel(201));
    }
}