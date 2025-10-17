using Finnisimo_Library_Backend.Domain.Entities.Loans;
using FluentValidation;

namespace Finnisimo_Library_Backend.Application.Commands.Loans.UpdateLoan;

public class UpdateLoanCommandValidator : AbstractValidator<UpdateLoanCommand>
{
  public UpdateLoanCommandValidator()
  {
    RuleFor(x => x.LoanId)
        .NotEmpty()
        .WithMessage("Loan ID is required to perform an update.");

    RuleFor(x => x.Status)
        .IsInEnum()
        .WithMessage("A valid loan status is required.");

    RuleFor(x => x.LoanDuration)
        .NotEmpty()
        .WithMessage("Loan duration is required when approving a loan.")
        .When(x => x.Status == LoanStatus.Approved);
  }
}
