using ErrorOr;
using Finnisimo_Library_Backend.Domain.Entities.Loans;
using Finnisimo_Library_Backend.Application.Abstractions.Gateways;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Loans.Repositories;
using Finnisimo_Library_Backend.Domain.Entities.Reservations.Repositories;

namespace Finnisimo_Library_Backend.Application.Commands.Loans.OfferExtensions;

internal sealed class OfferLoanExtensionsCommandHandler(
    ILoanRepository loanRepository,
    IReservationRepository reservationRepository,
    INotificationGateway notificationGateway, IUnitOfWork unitOfWork)
    : ICommandHandler<OfferLoanExtensionsCommand>
{
  public async Task<ErrorOr<None>> Handle(OfferLoanExtensionsCommand command,
                                          CancellationToken cancellationToken)
  {
    var upcomingLoans = await loanRepository.GetLoansDueInRangeAsync(
        DateTime.UtcNow, DateTime.UtcNow.AddHours(48), cancellationToken);

    foreach (var loan in upcomingLoans)
    {
      if (loan.ExtensionOfferedAt is not null ||
          loan.Status != LoanStatus.Approved)
      {
        continue;
      }

      var hasReservations =
          await reservationRepository.HasActiveReservationsForBookAsync(
              loan.BookId, cancellationToken);

      if (!hasReservations)
      {
        loan.OfferExtension();

        var payload = new
        {
          Message =
              $"Tienes una oferta para extender tu préstamo para el libro '{loan.Book.Title}'. Expira en 24 horas.",
          LoanId = loan.Id
        };
        await notificationGateway.SendNotificationAsync(
            loan.UserId, "ExtensionOffer", payload);
      }
    }

    await unitOfWork.SaveChangesAsync(cancellationToken);

    return new None();
  }
}
