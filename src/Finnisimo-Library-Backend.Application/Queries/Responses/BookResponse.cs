namespace Finnisimo_Library_Backend.Application.Queries.Responses;

public sealed class BookResponse
{
  public Guid Id { get; init; }
  public string Title { get; init; } = string.Empty;
  public string Author { get; init; } = string.Empty;
  public string Genre { get; init; } = string.Empty;
  public int Stock { get; init; }
  public string? CoverImageUrl { get; init; }
  public DateOnly PublicationDate { get; init; }
  public string Availability { get; init; } = string.Empty;
}
