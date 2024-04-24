using Calendar.Models;

namespace Calendar.Services;

public class DataService
{
    public string? Ip { get; set; }
    public string? Location { get; set; }

    public DateTime Date { get; set; }

    public List<EventModel> Events { get; set; } = [];

    public event EventHandler DataFetched;

    public virtual void OnDataFetched(EventArgs e)
    {
        DataFetched?.Invoke(this, e);
    }

    public event EventHandler LocalTimeChanged;

    public virtual void OnLocalTimeChanged(EventArgs e)
    {
        LocalTimeChanged?.Invoke(this, e);
    }

}