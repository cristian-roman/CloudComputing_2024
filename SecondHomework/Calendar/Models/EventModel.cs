namespace Calendar.Models;

public class EventModel
{
    public Guid id { get; set; }
    public string name { get; set; }
    public DateTime begins { get; set; }
    public DateTime ends { get; set; }
    public string description { get; set; }

}