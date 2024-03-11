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
    private static readonly string[] ExpectedProperties = ["name", "begins", "ends", "description"];

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
            ("Request body does not have the proper data types type:\n" +
             "\"name\" -> string\n" +
             "\"begins\" -> the ISO 8601 combined date-time format\n" +
             "\"ends\" -> the ISO 8601 combined date-time format\n" +
             "\"description\" -> optional string\n");
        }

        EventsRequestValidator.CheckRequiredFields(eventModel);
        await DB.DbCommands.Events.CreateEvent(eventModel!);

        if (!EventsRequestValidator.MoreDataThenRequiredDetected(request.Body!, ExpectedProperties))
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

        if (events.Count == 0)
            return new APIResponse("No events found.", new ResponseStatusModel(404));

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
            ("Request body does not have the proper data types:\n" +
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

        var id = (await ApiUtils.ExtractIdsFromPath(request))[0];
        eventModel.Id = new Guid(id);

        try
        {
            await DB.DbCommands.Events.ModifyEvent(eventModel);
        }
        catch (ResourceNotFoundException e)
        {
            return new APIResponse(e.Message, new ResponseStatusModel(404));
        }


        if (!EventsRequestValidator.MoreDataThenRequiredDetected(request.Body!, ExpectedProperties))
            return new APIResponse("Event modified successfully.", new ResponseStatusModel(200));

        return new APIResponse("Event modified successfully.\n" +
                               "Extra fields were detected, but they were ignored by the server",
            new ResponseStatusModel(200));
    }

    [Route("DELETE", "/event/{id}")]
    public static async Task<APIResponse> DeleteEvent(RequestModel request)
    {
        EventsRequestValidator.ValidateBodyUnfilled(request);

        var id = (await ApiUtils.ExtractIdsFromPath(request))[0];
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

    [Route("GET", "/event/{id}")]
    public static async Task<APIResponse> GetEvent(RequestModel request)
    {
        EventsRequestValidator.ValidateBodyUnfilled(request);

        var id = (await ApiUtils.ExtractIdsFromPath(request))[0];

        var dbEvent = await DB.DbCommands.Events.GetEvent(new Guid(id));

        return dbEvent == null ?
            new APIResponse("No event with the given id was found.", new ResponseStatusModel(404)) :
            new APIResponse(JsonSerializer.Serialize(dbEvent), new ResponseStatusModel(200));
    }

    [Route("PATCH", "/event/{id}")]
    public static async Task<APIResponse> PatchEventDescription(RequestModel request)
    {
        EventsRequestValidator.CheckNotNullBodyRequirement(request);

        var id = (await ApiUtils.ExtractIdsFromPath(request))[0];
        EventModel? eventModel = null;
        try
        {
            eventModel = request.Body!.Deserialize<EventModel>();
        }
        catch (JsonException)
        {
            throw new UnexpectedBodyException
            ("Request body does not have the proper data type:\n" +
             "\"description\" -> string\n" +
             "Only the description field is allowed to be modified with the patch method.");
        }

        if (eventModel!.Description == null)
        {
            throw new PropertyMissingException
                ("Request body requires a property named \"description\" of type string.");
        }

        if (EventsRequestValidator.MoreDataThenRequiredDetected(request.Body!, new List<string> {"description"}))
        {
            throw new UnexpectedBodyException
                ("Extra fields were detected. Patch method only allows the description field to be modified.");
        }

        eventModel.Id = new Guid(id);
        try
        {
            await DB.DbCommands.Events.PatchEventDescription(eventModel);
            return new APIResponse("Event description modified successfully.", new ResponseStatusModel(200));
        }
        catch (ResourceNotFoundException e)
        {
            return new APIResponse(e.Message, new ResponseStatusModel(404));
        }
    }

    /// <summary>
    /// Shows events that overlap with the given time frame.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>

    [Route("GET", "/events/{timestamp}:{timestamp}")]
    public static async Task<APIResponse> GetEventsByTime(RequestModel request)
    {
        EventsRequestValidator.ValidateBodyUnfilled(request);
        var timestamps = await ApiUtils.ExtractDatesFromPath(request);
        var events = await DB.DbCommands.Events.GetEventsByTime(timestamps[0], timestamps[1]);

        if(events.Count == 0)
            return new APIResponse("No events found in the given time frame.", new ResponseStatusModel(404));

        StringBuilder response = new();
        response.Append('[');
        foreach (var eventModel in events)
        {
            response.Append(JsonSerializer.Serialize(eventModel));
            response.Append(',');
        }

        response[^1] = ']';

        return new APIResponse(response.ToString(), new ResponseStatusModel(200));
    }

    /// <summary>
    /// Deletes events that are within the given time frame.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [Route("DELETE", "/events/{timestamp}:{timestamp}")]
    public static async Task<APIResponse> DeleteEventsByTime(RequestModel request)
    {
        EventsRequestValidator.ValidateBodyUnfilled(request);
        var timestamps = await ApiUtils.ExtractDatesFromPath(request);

        try
        {
            await DB.DbCommands.Events.DeleteEventsByTime(timestamps[0], timestamps[1]);
        }
        catch (ResourceNotFoundException e)
        {
            return new APIResponse(e.Message, new ResponseStatusModel(404));
        }

        return new APIResponse("Events deleted successfully.", new ResponseStatusModel(200));
    }

}