using Finnisimo_Library_Backend.Application.Services.Clock;

namespace Finnisimo_Library_Backend.Infrastructure.Services.Clock;

internal sealed class DateTimeService : IDateTimeService
{
  public DateTime CurrentTime => DateTime.UtcNow;
}
