using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MultiServer.Controllers.Calendar;

[Route("api/[controller]")]
[ApiController]
public class CalendarController : ControllerBase
{
    [HttpPost]
    [Route("events")]
    public async Task<IActionResult> CreateEvent([FromBody] EventModel newEvent)
    {
        using var httpClient = new HttpClient();

        // Serialize the event model to JSON
        var serializedEvent = JsonConvert.SerializeObject(newEvent);

        // Send the serialized event in the request body
        var response = await httpClient.PostAsync("http://localhost:5500/events",
            new StringContent(serializedEvent, Encoding.UTF8, "application/json"));

        // Return the response from the other server with the specific status code and message
        return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
    }

    [HttpGet]
    [Route("events")]
    public async Task<IActionResult> GetEvents()
    {
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync("http://localhost:5500/events");

        //return the response from the other server with the specific status code and message
        return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
    }

    [HttpGet]
    [Route("event/{id:guid}")]
    public async Task<IActionResult> GetEventById(Guid id)
    {
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync($"http://localhost:5500/event/{id}");

        //return the response from the other server with the specific status code and message
        return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
    }

    [HttpGet("events/overlapping_events")]
    public async Task<IActionResult> GetOverlappingEventsWithTheDateRange(
        [FromQuery(Name = "begins")] DateTime begins,
        [FromQuery(Name = "ends")] DateTime ends)
    {
        var httpClient = new HttpClient();
        var beginsFormatted = begins.ToString("yyyy-MM-ddTHH:mm:ss");
        var endsFormatted = ends.ToString("yyyy-MM-ddTHH:mm:ss");
        var response = await httpClient.GetAsync($"http://localhost:5500/events/{beginsFormatted}:{endsFormatted}");

        //return the response from the other server with the specific status code and message
        return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
    }

    [HttpPut]
    [Route("event/{id:guid}")]
    public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] EventModel updatedEvent)
    {
        using var httpClient = new HttpClient();

        // Serialize the event model to JSON
        var serializedEvent = JsonConvert.SerializeObject(updatedEvent);

        // Send the serialized event in the request body
        var response = await httpClient.PutAsync($"http://localhost:5500/event/{id}",
            new StringContent(serializedEvent, Encoding.UTF8, "application/json"));

        // Return the response from the other server with the specific status code and message
        return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
    }

    [HttpPatch]
    [Route("event/{id:guid}")]
    public async Task<IActionResult> PartiallyUpdateEvent(Guid id, [FromBody] EventPatchModel patchDocument)
    {
        using var httpClient = new HttpClient();

        // Serialize the patch document to JSON
        var serializedPatchDocument = JsonConvert.SerializeObject(patchDocument);

        // Send the serialized patch document in the request body
        var response = await httpClient.PatchAsync($"http://localhost:5500/event/{id}",
            new StringContent(serializedPatchDocument, Encoding.UTF8, "application/json"));

        // Return the response from the other server with the specific status code and message
        return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
    }

    [HttpDelete]
    [Route("event/{id:guid}")]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        var httpClient = new HttpClient();
        var response = await httpClient.DeleteAsync($"http://localhost:5500/event/{id}");

        // Return the response from the other server with the specific status code and message
        return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
    }

    [HttpDelete]
    [Route("events/in_range")]
    public async Task<IActionResult> DeleteEventsFromRange(
        [FromQuery(Name = "begins")] DateTime begins,
        [FromQuery(Name = "ends")] DateTime ends)
    {
        var httpClient = new HttpClient();
        var beginsFormatted = begins.ToString("yyyy-MM-ddTHH:mm:ss");
        var endsFormatted = ends.ToString("yyyy-MM-ddTHH:mm:ss");
        var response = await httpClient.DeleteAsync($"http://localhost:5500/events/{beginsFormatted}:{endsFormatted}");

        // Return the response from the other server with the specific status code and message
        return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
    }
}