using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;

namespace Finnisimo_Library_Backend.Application.Commands.Books.DeleteBook;

public record DeleteBookCommand(Guid BookId) : ICommand;
