namespace Finnisimo_Library_Backend.Domain.Abstractions;

public sealed class PaginedResponse<T>
{
  public IReadOnlyList<T> Items { get; init; } = [];
  public int TotalCount { get; init; }
  public int Page { get; init; }
  public int PageSize { get; init; }
}
