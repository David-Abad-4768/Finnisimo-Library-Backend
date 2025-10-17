namespace Finnisimo_Library_Backend.Application.Queries.Responses;

public sealed class UserLoanResponse
{
  public Guid Id { get; init; }
  public Guid IdBook { get; init; }
  public string Title { get; init; } = string.Empty;
  public string Author { get; init; } = string.Empty;
  public string? CoverImageUrl { get; init; }
  public int NumberOfPages { get; init; }
  public DateTime LoanedAt { get; init; }
  public DateTime DueDate { get; init; }
  public string Status { get; init; } = string.Empty;
  public string TimeRemainingFriendly { get; set; } = string.Empty;
  public TimeSpan TimeRemaining { get; set; }
}
