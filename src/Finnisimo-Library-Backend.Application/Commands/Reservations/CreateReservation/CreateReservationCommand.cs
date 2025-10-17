using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;

namespace Finnisimo_Library_Backend.Application.Commands.Reservations
    .CreateReservation;

public sealed record CreateReservationCommand(Guid BookId, Guid UserId,
                                              int DesiredLoanDurationInDays)
    : ICommand<Guid>;
