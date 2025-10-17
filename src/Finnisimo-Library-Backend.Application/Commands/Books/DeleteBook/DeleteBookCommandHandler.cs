using ErrorOr;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Books.Repositories;

namespace Finnisimo_Library_Backend.Application.Commands.Books.DeleteBook;

internal sealed class DeleteBookCommandHandler(IBookRepository bookRepository,
                                               IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteBookCommand>
{
  public async Task<ErrorOr<None>> Handle(DeleteBookCommand command,
                                          CancellationToken cancellationToken)
  {
    var book =
        await bookRepository.GetByIdAsync(command.BookId, cancellationToken);

    if (book is null)
    {
      return Error.NotFound(
          "Book.NotFound",
          $"The book with the ID '{command.BookId}' was not found.");
    }

    await bookRepository.DeleteAsync(book, cancellationToken);

    await unitOfWork.SaveChangesAsync(cancellationToken);

    return new None();
  }
}
