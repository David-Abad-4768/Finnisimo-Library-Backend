using ErrorOr;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Loans;
using Finnisimo_Library_Backend.Domain.Entities.Loans.Repositories;

namespace Finnisimo_Library_Backend.Application.Commands.Loans.UpdateLoan;

internal sealed class UpdateLoanCommandHandler(ILoanRepository loanRepository,
                                               IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateLoanCommand>
{
  public async Task<ErrorOr<None>> Handle(UpdateLoanCommand command,
                                          CancellationToken cancellationToken)
  {
    var loan =
        await loanRepository.GetByIdAsync(command.LoanId, cancellationToken);

    if (loan is null)
    {
      return Error.NotFound(
          "Loan.NotFound",
          $"The loan with ID '{command.LoanId}' was not found.");
    }

    try
    {
      switch (command.Status)
      {
        case LoanStatus.Approved:
          loan.Approve(DateTime.UtcNow, command.LoanDuration!.Value);
          break;

        case LoanStatus.Rejected:
          loan.Reject();
          break;

        case LoanStatus.Returned:
          loan.MarkAsReturned(DateTime.UtcNow);
          break;

        default:
          return Error.Validation(
              "Loan.InvalidStatusUpdate",
              "This status cannot be manually set via this command.");
      }
    }
    catch (InvalidOperationException ex)
    {
      return Error.Validation("Loan.InvalidStateTransition", ex.Message);
    }

    await loanRepository.UpdateAsync(loan, cancellationToken);

    await unitOfWork.SaveChangesAsync(cancellationToken);

    return new None();
  }
}
