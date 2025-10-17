using FluentValidation;

namespace Finnisimo_Library_Backend.Application.Commands.Loans.CancelLoan;

public class CancelLoanCommandValidator : AbstractValidator<CancelLoanCommand>
{
  public CancelLoanCommandValidator()
  {
    RuleFor(x => x.LoanId)
        .NotEmpty()
        .WithMessage("Loan ID is required to cancel a loan.");

    RuleFor(x => x.UserId)
        .NotEmpty()
        .WithMessage("User ID is required for validation.");
  }
}
