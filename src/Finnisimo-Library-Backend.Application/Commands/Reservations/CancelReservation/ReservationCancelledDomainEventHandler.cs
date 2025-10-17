using MediatR;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Books.Repositories;
using Finnisimo_Library_Backend.Domain.Entities.Reservations.Events;
using Finnisimo_Library_Backend.Domain.Entities.Reservations.Repositories;

namespace Finnisimo_Library_Backend.Application.Commands.Reservations
    .CancelReservation;

public class ReservationCancelledDomainEventHandler(
    IReservationRepository reservationRepository,
    IBookRepository bookRepository, IUnitOfWork unitOfWork)
    : INotificationHandler<ReservationCancelledDomainEvent>
{
  public async Task Handle(ReservationCancelledDomainEvent notification,
                           CancellationToken cancellationToken)
  {
    var book = await bookRepository.GetByIdAsync(notification.BookId,
                                                 cancellationToken);

    if (book is null || book.Stock < 1)
    {
      return;
    }

    var nextReservation =
        await reservationRepository.GetNextActiveReservationForBookAsync(
            notification.BookId, cancellationToken);

    if (nextReservation is null)
    {
      return;
    }

    nextReservation.Fulfill();

    await unitOfWork.SaveChangesAsync(cancellationToken);
  }
}
