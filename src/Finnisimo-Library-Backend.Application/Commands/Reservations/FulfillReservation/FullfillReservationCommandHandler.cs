using ErrorOr;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;
using Finnisimo_Library_Backend.Domain.Entities.Reservations.Repositories;
using Finnisimo_Library_Backend.Domain.Abstractions;

namespace Finnisimo_Library_Backend.Application.Commands.Reservations
    .FulfillReservation;

internal sealed class FulfillReservationCommandHandler(
    IReservationRepository reservationRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<FulfillReservationCommand, Guid>
{
  private readonly IReservationRepository _reservationRepository =
      reservationRepository;

  public async Task<ErrorOr<Guid>> Handle(FulfillReservationCommand request,
                                          CancellationToken cancellationToken)
  {
    var reservation =
        await _reservationRepository.GetActiveReservationByUserAndBookAsync(
            request.UserId, request.BookId, cancellationToken);

    if (reservation is null)
    {
      return Error.NotFound(code: "Reservation.NotFound",
                            description: "Active reservation not found for " +
                                "the specified user and book.");
    }

    reservation.Fulfill();

    await unitOfWork.SaveChangesAsync(cancellationToken);

    return reservation.Id;
  }
}
