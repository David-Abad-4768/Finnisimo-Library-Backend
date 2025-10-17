using MediatR;
using Finnisimo_Library_Backend.Application.Commands.Reservations
    .FulfillReservation;
using Finnisimo_Library_Backend.Domain.Entities.Loans.Events;
using Finnisimo_Library_Backend.Domain.Entities.Reservations.Repositories;

namespace Finnisimo_Library_Backend.Application.DomainEventHandlers.Loans;

public class LoanReturnedEventHandler(
    IReservationRepository reservationRepository, ISender mediator)
    : INotificationHandler<LoanReturnedDomainEvent>
{
  public async Task Handle(LoanReturnedDomainEvent notification,
                           CancellationToken cancellationToken)
  {
    var nextReservation =
        await reservationRepository.GetNextActiveReservationForBookAsync(
            notification.BookId, cancellationToken);

    if (nextReservation is null)
    {
      return;
    }

    await mediator.Send(new FulfillReservationCommand(nextReservation.BookId,
                                                      nextReservation.UserId),
                        cancellationToken);
  }
}
