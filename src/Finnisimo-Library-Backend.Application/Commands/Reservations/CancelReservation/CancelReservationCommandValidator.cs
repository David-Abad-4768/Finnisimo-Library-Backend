using FluentValidation;

namespace Finnisimo_Library_Backend.Application.Commands.Reservations
    .CancelReservation;

public class CancelReservationCommandValidator
    : AbstractValidator<CancelReservationCommand>
{
  public CancelReservationCommandValidator()
  {
    RuleFor(x => x.ReservationId)
        .NotEmpty()
        .WithMessage("Reservation ID is required.");

    RuleFor(x => x.UserId)
        .NotEmpty()
        .WithMessage("User ID is required for validation.");
  }
}
