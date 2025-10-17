namespace Finnisimo_Library_Backend.Application.Queries.Responses;

public sealed class FinishedBookResponse
{
  public string Title { get; init; } = string.Empty;
  public DateTime FinishedDate { get; init; }
}
