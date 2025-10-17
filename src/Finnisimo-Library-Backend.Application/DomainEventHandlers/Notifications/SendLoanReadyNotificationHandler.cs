using MediatR;
using Finnisimo_Library_Backend.Application.Abstractions.Gateways;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Books.Repositories;
using Finnisimo_Library_Backend.Domain.Entities.Notifications;
using Finnisimo_Library_Backend.Domain.Entities.Notifications.Repositories;
using Finnisimo_Library_Backend.Domain.Entities.Reservations.Events;

namespace Finnisimo_Library_Backend.Application.DomainEventHandlers
    .Notifications;

public class SendLoanReadyNotificationHandler(
    INotificationGateway notificationGateway, IBookRepository bookRepository,
    INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
    : INotificationHandler<ReservationFulfilledDomainEvent>
{
  public async Task Handle(ReservationFulfilledDomainEvent notification,
                           CancellationToken cancellationToken)
  {
    var book = await bookRepository.GetByIdAsync(notification.BookId,
                                                 cancellationToken);
    if (book is null)
      return;

    var notificationPayload = new
    {
      title = "Reservación Lista",
      message =
          $"¡Tu reservación para el libro '{book.Title}' está lista! Hemos creado un préstamo a tu nombre."
    };

    var newNotification = NotificationEntity.Create(
        notification.UserId, notificationPayload.title,
        notificationPayload.message);

    await notificationRepository.AddAsync(newNotification, cancellationToken);
    await unitOfWork.SaveChangesAsync(cancellationToken);

    await notificationGateway.SendLoanReadyNotificationAsync(
        notification.UserId, notificationPayload, cancellationToken);
  }
}
