using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace MultiServer.Controllers.Weather;

[Route("api/[controller]")]
[ApiController]
public class WeatherController(IConfiguration configuration) : ControllerBase
{
    [HttpGet]
    [Route("current")]
    public async Task<IActionResult> GetWeatherForecast(
        [FromQuery(Name = "city")] string city)
    {
        //the weather api key is stored in the dotnet user-secrets by using CLI :
        //dotnet user-secrets set "WeatherApiKey" "your_key_here"

        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync($"http://api.weatherapi.com/v1/current.json?" +
                                                 $"key={configuration["WeatherApiKey"]}&q={city}");

        var statusCode = (int)response.StatusCode;
        if (statusCode != 200)
        {
            return StatusCode(statusCode, "City not found");
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        try
        {
            var responseJson = JObject.Parse(responseContent);
            var localTime = responseJson["location"]["localtime"].Value<string>();
            var temperature = responseJson["current"]["temp_c"].Value<double>();
            var conditionText = responseJson["current"]["condition"]["text"].Value<string>();
            var imageUrl = responseJson["current"]["condition"]["icon"].Value<string>();

            var jsonMessage = new JObject
            {
                ["localTime"] = localTime,
                ["temperature"] = temperature,
                ["conditionText"] = conditionText,
                ["imageUrl"] = imageUrl
            };

            return Ok(jsonMessage.ToString());
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    [Route("forecast")]
    public async Task<IActionResult> GetWeatherForecast(
        [FromQuery(Name = "city")] string city,
        [FromQuery(Name = "days")] int days)
    {
        //the weather api key is stored in the dotnet user-secrets by using CLI :
        //dotnet user-secrets set "WeatherKey" "your_key_here"

        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync($"http://api.weatherapi.com/v1/forecast.json?" +
                                                 $"key={configuration["WeatherApiKey"]}&q={city}&days={days}");

        return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
    }
}