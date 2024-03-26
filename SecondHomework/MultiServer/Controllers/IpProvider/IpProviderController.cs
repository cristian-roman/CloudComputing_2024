using Microsoft.AspNetCore.Mvc;

namespace MultiServer.Controllers.IpProvider;

[Route("api/[controller]")]
[ApiController]
public class IpProviderController : ControllerBase
{
    [HttpGet]
    [Route("my_ip")]
    public async Task<IActionResult> GetPublicAddress()
    {
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync("https://api.ipify.org?format=json");

        return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
    }
}