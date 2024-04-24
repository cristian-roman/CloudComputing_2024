using System.Text.Json;
using FirstHomework.DB.DbCommands;
using FirstHomework.Network.Resolver.RequestProcessor;
using FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Body;
using FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Request;

namespace FirstHomework.APIs.Events;

public static class EventsRequestValidator
{
    public static void CheckNotNullBodyRequirement(RequestModel request)
    {
        if (request.Body == null)
        {
            throw new EmptyRequestBodyException
            ("Request body is empty. Please provide:" +
             "\"name\" -> the name of the event,\n" +
             "\"begins\" -> the ISO 8601 combined date-time format of the event start,\n" +
             "\"ends\" -> the ISO 8601 combined date-time format of the event end,\n" +
             "\"description\" -> parameter for the description of the event. Optional when POST operation in use, mandatory for PUT\n");
        }
    }

    public static void CheckRequiredFields(EventModel? eventModel)
    {
        if (eventModel == null)
        {
            throw new Exception
                ("Server could not deserialize the request body, " +
                 "although it could be parsed as a JsonDocument when received." +
                 "This is an internal server error and should be investigated.");
        }

        if (eventModel.EventName == null)
        {
            throw new PropertyMissingException
                ("Request body requires a property named \"name\" -> the name of the event.");
        }

        if (eventModel.Begins == null)
        {
            throw new PropertyMissingException
                ("Request body requires a property named \"begins\" " +
                 "-> the ISO 8601 combined date-time format of the event start.");
        }

        if (eventModel.Ends == null)
        {
            throw new PropertyMissingException
                ("Request body requires a property named \"ends\" " +
                 "-> the ISO 8601 combined date-time format of the event end.");
        }
    }

    public static bool MoreDataThenRequiredDetected(JsonDocument eventJsonModel, IEnumerable<string> expectedProperties)
    {
        var detectedProperties = eventJsonModel.RootElement.EnumerateObject().Select(property => property.Name).ToList();

        return detectedProperties.Except(expectedProperties).Any();
    }

    public static void ValidateBodyUnfilled(RequestModel request)
    {
        if (request.Body != null)
        {
            throw new UnexpectedBodyException
            ("Request body is not expected for this request.");
        }
    }
}