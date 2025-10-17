namespace Finnisimo_Library_Backend.Application.Queries.Responses;

public sealed class UserReservationResponse
{
  public Guid Id { get; init; }
  public string BookTitle { get; init; } = string.Empty;
  public string? CoverImageUrl { get; init; }
  public DateTime ReservationDate { get; init; }
  public string Status { get; init; } = string.Empty;
}
