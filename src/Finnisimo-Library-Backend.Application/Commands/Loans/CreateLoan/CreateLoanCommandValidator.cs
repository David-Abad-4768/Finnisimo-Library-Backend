using FluentValidation;

namespace Finnisimo_Library_Backend.Application.Commands.Loans.CreateLoan;

public class CreateLoanCommandValidator : AbstractValidator<CreateLoanCommand>
{
  public CreateLoanCommandValidator()
  {
    RuleFor(x => x.UserId)
        .NotEmpty()
        .WithMessage("User ID is required to create a loan.");

    RuleFor(x => x.BookId)
        .NotEmpty()
        .WithMessage("Book ID is required to create a loan.");

    RuleFor(x => x.LoanDurationInDays)
        .GreaterThan(0)
        .WithMessage("Loan duration must be at least 1 day.");
  }
}
