using System.Reflection;
using System.Text;
using System.Text.Json;
using FirstHomework.DB.DbCommands;
using FirstHomework.DB.Exceptions;
using FirstHomework.Network.Resolver.RequestProcessor;
using FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Body;
using FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Request;
using FirstHomework.Network.Resolver.RequestRouter;
using FirstHomework.Network.Sender.Response;

namespace FirstHomework.APIs.Events;

public static class EventsAPI
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

        return new APIResponse("Event created successfully.\n" + warningMessage, new ResponseStatusModel(201));
    }

    [Route("GET", "/events")]
    public static async Task<APIResponse> GetEvents(RequestModel request)
    {
        EventsRequestValidator.ValidateBodyUnfilled(request);
        var events = await DB.DbCommands.Events.GetEvents();

        StringBuilder response = new();
        response.Append('[');
        foreach (var eventModel in (List<EventModel>)events)
        {
            response.Append(JsonSerializer.Serialize(eventModel));
            response.Append(',');
        }

        response[^1] = ']';

        return new APIResponse(response.ToString(), new ResponseStatusModel(200));
    }

    [Route("PUT", "/event/{id}")]
    public static async Task<APIResponse> ModifyEvent(RequestModel request)
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
             "\"description\" -> string\n");
        }

        EventsRequestValidator.CheckRequiredFields(eventModel);
        if (eventModel!.Description == null)
        {
            throw new PropertyMissingException
                ("Request body requires a property named \"description\" " +
                 "-> parameter for the description of the event. Optional when POST operation in use, mandatory for PUT\n");
        }

        var id = (await APIUtils.ExtractIdsFromPath(request))[0];
        eventModel.Id = new Guid(id);

        try
        {
            await DB.DbCommands.Events.ModifyEvent(eventModel);
        }
        catch (ResourceNotFoundException e)
        {
            return new APIResponse(e.Message, new ResponseStatusModel(404));
        }


        if (!EventsRequestValidator.MoreDataThenRequiredDetected(request.Body!))
            return new APIResponse("Event modified successfully.", new ResponseStatusModel(200));

        return new APIResponse("Event modified successfully.\n" +
                               "Extra fields were detected, but they were ignored by the server",
            new ResponseStatusModel(200));
    }

    [Route("DELETE", "/event/{id}")]
    public static async Task<APIResponse> DeleteEvent(RequestModel request)
    {
        EventsRequestValidator.ValidateBodyUnfilled(request);

        var id = (await APIUtils.ExtractIdsFromPath(request))[0];
        try
        {
            await DB.DbCommands.Events.DeleteEvent(new Guid(id));
        }
        catch (ResourceNotFoundException e)
        {
            return new APIResponse(e.Message, new ResponseStatusModel(404));
        }
        return new APIResponse("Event deleted successfully.", new ResponseStatusModel(200));
    }
}