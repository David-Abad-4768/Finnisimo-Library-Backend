namespace Finnisimo_Library_Backend.WebApi.Controllers.v1.Book.Request;

public record CreateBookRequest(string Title, string Author, string Publisher,
                                DateOnly PublicationDate, string Description,
                                string Genre, int NumberOfPages,
                                string Language, string? CoverImageUrl,
                                int Stock, string Location);
