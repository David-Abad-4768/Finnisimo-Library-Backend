using ErrorOr;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Loans.Repositories;
using Finnisimo_Library_Backend.Domain.Entities.Reservations.Repositories;

namespace Finnisimo_Library_Backend.Application.Commands.Loans.RequestExtension;

internal sealed class RequestLoanExtensionCommandHandler(
    ILoanRepository loanRepository,
    IReservationRepository reservationRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<RequestLoanExtensionCommand, string>
{
  public async Task<ErrorOr<string>>
  Handle(RequestLoanExtensionCommand command,
         CancellationToken cancellationToken)
  {
    var loan =
        await loanRepository.GetByIdAsync(command.LoanId, cancellationToken);
    if (loan is null)
      return Error.NotFound("Loan.NotFound", "Loan not found.");
    if (loan.UserId != command.UserId)
      return Error.Forbidden("Loan.AccessDenied",
                             "You cannot extend this loan.");

    var hasReservations =
        await reservationRepository.HasActiveReservationsForBookAsync(
            loan.BookId, cancellationToken);
    if (hasReservations)
    {
      return Error.Conflict("Loan.BookIsReserved",
                            "The loan cannot be extended because the book is " +
                                "reserved by another user.");
    }

    try
    {
      var extensionDuration = TimeSpan.FromDays(command.ExtensionInDays);
      loan.Extend(extensionDuration);
    }
    catch (InvalidOperationException ex)
    {
      return Error.Conflict("Loan.NotExtendable", ex.Message);
    }

    await unitOfWork.SaveChangesAsync(cancellationToken);

    return $"Loan successfully extended until {loan.DueDate:d}.";
  }
}
