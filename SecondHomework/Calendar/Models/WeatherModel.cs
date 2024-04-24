namespace Calendar.Models;

public class WeatherModel
{
    public DateTime CurrentTime { get; set; }
    public float Temperature { get; set; }
    public string ConditionText { get; set; }
    public string ImageUrl { get; set; }
}