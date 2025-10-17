using FluentValidation;

namespace Finnisimo_Library_Backend.Application.Commands.Books.CreateBook;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
  public CreateBookCommandValidator()
  {
    RuleFor(x => x.Title)
        .NotEmpty()
        .WithMessage("Title is required.")
        .MaximumLength(200)
        .WithMessage("Title must not exceed 200 characters.");

    RuleFor(x => x.Author)
        .NotEmpty()
        .WithMessage("Author is required.")
        .MaximumLength(100)
        .WithMessage("Author name must not exceed 100 characters.");

    RuleFor(x => x.Publisher)
        .NotEmpty()
        .WithMessage("Publisher is required.")
        .MaximumLength(100)
        .WithMessage("Publisher name must not exceed 100 characters.");

    RuleFor(x => x.Description)
        .NotEmpty()
        .WithMessage("Description is required.")
        .MaximumLength(1000)
        .WithMessage("Description must not exceed 1000 characters.");

    RuleFor(x => x.Genre)
        .NotEmpty()
        .WithMessage("Genre is required.")
        .MaximumLength(50)
        .WithMessage("Genre must not exceed 50 characters.");

    RuleFor(x => x.NumberOfPages)
        .GreaterThan(0)
        .WithMessage("Number of pages must be greater than zero.");

    RuleFor(x => x.Stock)
        .GreaterThanOrEqualTo(0)
        .WithMessage("Stock cannot be negative.");

    RuleFor(x => x.Location)
        .NotEmpty()
        .WithMessage("Location is required.")
        .MaximumLength(100)
        .WithMessage("Location must not exceed 100 characters.");
  }
}
