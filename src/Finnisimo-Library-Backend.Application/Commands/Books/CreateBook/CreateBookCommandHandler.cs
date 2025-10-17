using ErrorOr;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Books;
using Finnisimo_Library_Backend.Domain.Entities.Books.Repositories;

namespace Finnisimo_Library_Backend.Application.Commands.Books.CreateBook;

internal sealed class CreateBookCommandHandler(IBookRepository bookRepository,
                                               IUnitOfWork unitOfWork)
    : ICommandHandler<CreateBookCommand, string>
{
  public async Task<ErrorOr<string>>
  Handle(CreateBookCommand command, CancellationToken cancellationToken)
  {
    if (await bookRepository.DoesBookExistAsync(command.Title, command.Author,
                                                cancellationToken))
    {
      return Error.Conflict(
          "Book.AlreadyExists",
          "A book with this title and author already exists.");
    }

    var book = BookEntity.Create(
        command.Title, command.Author, command.Publisher,
        command.PublicationDate, command.Description, command.Genre,
        command.NumberOfPages, command.Language, command.CoverImageUrl,
        command.Stock, command.Location);

    await bookRepository.AddAsync(book, cancellationToken);

    await unitOfWork.SaveChangesAsync(cancellationToken);

    return "Book request created successfully";
  }
}
