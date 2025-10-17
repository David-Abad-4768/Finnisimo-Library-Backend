namespace Finnisimo_Library_Backend.Application.Queries.Responses;

public sealed class BookPagesReadResponse
{
  public string Title { get; init; } = string.Empty;
  public int PagesRead { get; init; }
}
