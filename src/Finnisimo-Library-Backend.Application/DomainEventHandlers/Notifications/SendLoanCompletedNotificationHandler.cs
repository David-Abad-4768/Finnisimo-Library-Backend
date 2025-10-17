using MediatR;
using Finnisimo_Library_Backend.Application.Abstractions.Gateways;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Books.Repositories;
using Finnisimo_Library_Backend.Domain.Entities.Loans.Events;
using Finnisimo_Library_Backend.Domain.Entities.Notifications;
using Finnisimo_Library_Backend.Domain.Entities.Notifications.Repositories;

namespace Finnisimo_Library_Backend.Application.DomainEventHandlers
    .Notifications;

public class SendLoanCompletedNotificationHandler(
    INotificationGateway notificationGateway, IBookRepository bookRepository,
    INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
    : INotificationHandler<LoanReturnedDomainEvent>
{
  public async Task Handle(LoanReturnedDomainEvent notification,
                           CancellationToken cancellationToken)
  {
    var book = await bookRepository.GetByIdAsync(notification.BookId,
                                                 cancellationToken);
    if (book is null)
      return;

    var payload = new
    {
      title = "Préstamo Finalizado",
      message =
          $"El período de préstamo para el libro '{book.Title}' ha finalizado automáticamente."
    };

    var newNotification = NotificationEntity.Create(
        notification.UserId, payload.title, payload.message);

    await notificationRepository.AddAsync(newNotification, cancellationToken);
    await unitOfWork.SaveChangesAsync(cancellationToken);

    await notificationGateway.SendLoanReadyNotificationAsync(
        notification.UserId, payload, cancellationToken);
  }
}
