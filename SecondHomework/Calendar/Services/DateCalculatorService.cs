namespace Calendar.Services;

public class DateCalculatorService
{
    public DateTime CalculateFirstDayOfWeekAsync(DateTime now)
    {
        var firstDayOfWeek = now.AddDays(-(int)now.DayOfWeek);

        //return it with time 00:00:00
        return new DateTime(firstDayOfWeek.Year, firstDayOfWeek.Month, firstDayOfWeek.Day);
    }

    public DateTime CalculateLastDayOfWeekAsync(DateTime now)
    {
        var lastDayOfWeek = now.AddDays(6 - (int)now.DayOfWeek);

        //return it with time 23:59:59
        return new DateTime(lastDayOfWeek.Year, lastDayOfWeek.Month, lastDayOfWeek.Day, 23, 59, 59);
    }

    public DateTime DayOfTheWeekToDateTime(string dayOfTheWeek, DateTime now)
    {
        DayOfWeek dw = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayOfTheWeek);

        var day = now.DayOfWeek - dw;
        if (day < 0)
        {
            day += 7;
        }

        return now.AddDays(-day);
    }
}