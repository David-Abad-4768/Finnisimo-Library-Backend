using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;

namespace Finnisimo_Library_Backend.Application.Commands.Books.CreateBook;

public record CreateBookCommand(string Title, string Author, string Publisher,
                                DateOnly PublicationDate, string Description,
                                string Genre, int NumberOfPages,
                                string Language, string? CoverImageUrl,
                                int Stock, string Location)
    : ICommand<string>;
