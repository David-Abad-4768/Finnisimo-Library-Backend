using ErrorOr;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Reservations.Repositories;

namespace Finnisimo_Library_Backend.Application.Commands.Reservations
    .CancelReservation;

internal sealed class CancelReservationCommandHandler(
    IReservationRepository reservationRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CancelReservationCommand>
{
  public async Task<ErrorOr<None>> Handle(CancelReservationCommand command,
                                          CancellationToken cancellationToken)
  {
    var reservation = await reservationRepository.GetByIdAsync(
        command.ReservationId, cancellationToken);

    if (reservation is null)
    {
      return Error.NotFound(
          code: "Reservation.NotFound",
          description: $"Reservation with ID {command.ReservationId} not found.");
    }

    if (reservation.UserId != command.UserId)
    {
      return Error.Forbidden(code: "Reservation.AccessDenied",
                             description: "You do not have permission to " +
                                 "cancel this reservation.");
    }

    try
    {
      reservation.Cancel();
    }
    catch (InvalidOperationException ex)
    {
      return Error.Validation(code: "Reservation.CannotBeCancelled",
                              description: ex.Message);
    }

    await unitOfWork.SaveChangesAsync(cancellationToken);

    return new None();
  }
}
