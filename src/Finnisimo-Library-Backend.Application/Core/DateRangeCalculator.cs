using Finnisimo_Library_Backend.Domain.Entities.ReadingLogs;

namespace Finnisimo_Library_Backend.Application.Core;

public static class DateRangeCalculator
{
  public static (DateTime startDate, DateTime endDate)
      Calculate(DateTime target, TimePeriod period)
  {
    return period switch
    {
      TimePeriod.Day => (target.Date, target.Date.AddDays(1).AddTicks(-1)),
      TimePeriod.Week => (target.Date.AddDays(-(int)target.DayOfWeek),
                          target.Date.AddDays(6 - (int)target.DayOfWeek)
                              .AddDays(1)
                              .AddTicks(-1)),
      TimePeriod.Month => (new DateTime(target.Year, target.Month, 1),
                           new DateTime(target.Year, target.Month, 1)
                               .AddMonths(1)
                               .AddDays(-1)
                               .AddDays(1)
                               .AddTicks(-1)),
      _ => (target.Date, target.Date.AddDays(1).AddTicks(-1)),
    };
  }
}
