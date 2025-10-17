namespace Finnisimo_Library_Backend.Application.Queries.Responses;

public record UserWishlistResponse(Guid BookId, string Title, string Author,
                                   string? CoverImageUrl, DateTime AddedAt);
