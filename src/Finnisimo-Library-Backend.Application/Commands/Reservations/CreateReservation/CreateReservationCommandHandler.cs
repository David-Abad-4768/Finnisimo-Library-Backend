using ErrorOr;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Books.Repositories;
using Finnisimo_Library_Backend.Domain.Entities.Reservations;
using Finnisimo_Library_Backend.Domain.Entities.Reservations.Repositories;

namespace Finnisimo_Library_Backend.Application.Commands.Reservations
    .CreateReservation;

internal sealed class CreateReservationCommandHandler(
    IReservationRepository reservationRepository,
    IBookRepository bookRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateReservationCommand, Guid>
{
  public async Task<ErrorOr<Guid>> Handle(CreateReservationCommand command,
                                          CancellationToken cancellationToken)
  {
    var book =
        await bookRepository.GetByIdAsync(command.BookId, cancellationToken);
    if (book is null)
    {
      return Error.NotFound(
          code: "Book.NotFound",
          description: $"The book with ID {command.BookId} was not found.");
    }

    if (book.Stock > 0)
    {
      return Error.Validation(
          code: "Reservation.BookIsAvailable",
          description: "Cannot reserve a book that is currently in stock " +
              "and available for loan.");
    }

    var existingReservation =
        await reservationRepository.HasActiveReservationAsync(
            command.UserId, command.BookId, cancellationToken);

    if (existingReservation)
    {
      return Error.Conflict(code: "Reservation.AlreadyExists",
                            description: "You already have an active " +
                                "reservation for this book.");
    }

    var reservation = ReservationEntity.Create(
        command.BookId, command.UserId, command.DesiredLoanDurationInDays);

    await reservationRepository.AddAsync(reservation, cancellationToken);

    await unitOfWork.SaveChangesAsync(cancellationToken);

    return reservation.Id;
  }
}
