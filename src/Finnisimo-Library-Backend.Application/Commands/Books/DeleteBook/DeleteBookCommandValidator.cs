using FluentValidation;

namespace Finnisimo_Library_Backend.Application.Commands.Books.DeleteBook;

public class DeleteBookCommandValidator : AbstractValidator<DeleteBookCommand>
{
  public DeleteBookCommandValidator()
  {
    RuleFor(x => x.BookId).NotEmpty().WithMessage("Book ID is required.");
  }
}
