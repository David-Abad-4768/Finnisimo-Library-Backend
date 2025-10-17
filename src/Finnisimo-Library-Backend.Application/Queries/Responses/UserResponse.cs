namespace Finnisimo_Library_Backend.Application.Queries.Responses;

public sealed class UserResponse
{
  public string? Name { get; init; }
  public string? LastName { get; init; }
  public string? Email { get; init; }
  public string? Username { get; init; }
  public DateTime DateOfBirth { get; init; }
  public string? PhotoUrl { get; init; }
}
