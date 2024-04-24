using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace MultiServer.Controllers.Location
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public LocationController()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("PostmanRuntime/7.37.0");
        }

        [HttpGet]
        [Route("my_location")]
        public async Task<IActionResult> GetLocation(
            [FromQuery(Name = "ip")] string ip)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var response = await _httpClient.GetAsync($"https://ipapi.co/{ip}/json/");

                if (!response.IsSuccessStatusCode)
                    return BadRequest($"Failed to get location data. Status code: {response.StatusCode}");
                var responseBody = await response.Content.ReadAsStringAsync();

                var json = JObject.Parse(responseBody);
                var city = json["city"];

                if (city==null || string.IsNullOrEmpty(city.ToString()))
                    return BadRequest("Failed to get location data. City not found.");

                var jsonAnswer = new JObject
                {
                    ["city"] = city.ToString()
                };

                return Ok(jsonAnswer.ToString());

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}