namespace Finnisimo_Library_Backend.Application.Queries.Responses;

public sealed class ReadingFrequencyResponse
{
  public DateOnly Date { get; init; }
  public int PagesRead { get; init; }
}
