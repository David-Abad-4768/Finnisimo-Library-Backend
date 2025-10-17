using ErrorOr;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Books.Repositories;
using Finnisimo_Library_Backend.Domain.Entities.Loans;
using Finnisimo_Library_Backend.Domain.Entities.Loans.Repositories;

namespace Finnisimo_Library_Backend.Application.Commands.Loans.CancelLoan;

internal sealed class CancelLoanCommandHandler(ILoanRepository loanRepository,
                                               IBookRepository bookRepository,
                                               IUnitOfWork unitOfWork)
    : ICommandHandler<CancelLoanCommand, string>
{
  public async Task<ErrorOr<string>>
  Handle(CancelLoanCommand command, CancellationToken cancellationToken)
  {
    var loan =
        await loanRepository.GetByIdAsync(command.LoanId, cancellationToken);

    if (loan is null)
    {
      return Error.NotFound(
          "Loan.NotFound",
          $"The loan with ID '{command.LoanId}' was not found.");
    }

    if (loan.UserId != command.UserId)
    {
      return Error.Forbidden("Loan.AccessDenied",
                             "You are not authorized to cancel this loan.");
    }

    if (loan.Status != LoanStatus.Approved)
    {
      return Error.Conflict(
          "Loan.NotCancellable",
          $"This loan cannot be cancelled because its status is '{loan.Status}', not 'Approved'.");
    }

    var book =
        await bookRepository.GetByIdAsync(loan.BookId, cancellationToken);

    if (book is null)
    {
      return Error.NotFound(
          "Book.NotFound", "The book associated with this loan was not found.");
    }

    loan.Cancel();
    book.IncreaseStock(1);

    await unitOfWork.SaveChangesAsync(cancellationToken);

    return "Loan cancelled successfully";
  }
}
