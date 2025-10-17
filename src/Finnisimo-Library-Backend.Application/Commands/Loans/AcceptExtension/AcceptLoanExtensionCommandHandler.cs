using ErrorOr;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Loans.Repositories;

namespace Finnisimo_Library_Backend.Application.Commands.Loans.AcceptExtension;

internal sealed class AcceptLoanExtensionCommandHandler(
    ILoanRepository loanRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<AcceptLoanExtensionCommand, string>
{
  private static readonly TimeSpan DefaultExtensionDuration =
      TimeSpan.FromDays(7);

  public async Task<ErrorOr<string>>
  Handle(AcceptLoanExtensionCommand command,
         CancellationToken cancellationToken)
  {
    var loan =
        await loanRepository.GetByIdAsync(command.LoanId, cancellationToken);

    if (loan is null)
      return Error.NotFound("Loan.NotFound", "Loan not found.");
    if (loan.UserId != command.UserId)
      return Error.Forbidden("Loan.AccessDenied",
                             "You cannot accept this offer.");

    try
    {
      loan.AcceptExtension(DefaultExtensionDuration);
    }
    catch (InvalidOperationException ex)
    {
      return Error.Conflict("Loan.OfferInvalid", ex.Message);
    }

    await unitOfWork.SaveChangesAsync(cancellationToken);

    return $"Loan successfully extended until {loan.DueDate:d}.";
  }
}
