namespace Finnisimo_Library_Backend.Application.Queries.Responses;

public sealed class BookDetailResponse
{
  public Guid Id { get; init; }
  public string Title { get; init; } = string.Empty;
  public string Author { get; init; } = string.Empty;
  public string Publisher { get; init; } = string.Empty;
  public DateOnly PublicationDate { get; init; }
  public string Description { get; init; } = string.Empty;
  public string Genre { get; init; } = string.Empty;
  public int NumberOfPages { get; init; }
  public string Language { get; init; } = string.Empty;
  public string? CoverImageUrl { get; init; }
  public int Stock { get; init; }
  public string Location { get; init; } = string.Empty;
  public int TimesLoaned { get; init; }
  public string Availability { get; init; } = string.Empty;
}
