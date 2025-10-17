using FluentValidation;

namespace Finnisimo_Library_Backend.Application.Commands.Reservations
    .CreateReservation;

public class CreateReservationCommandValidator
    : AbstractValidator<CreateReservationCommand>
{
  public CreateReservationCommandValidator()
  {
    RuleFor(x => x.BookId).NotEmpty();
    RuleFor(x => x.UserId).NotEmpty();

    RuleFor(x => x.DesiredLoanDurationInDays)
        .GreaterThan(0)
        .WithMessage("Loan duration must be at least 1 day.");
  }
}
