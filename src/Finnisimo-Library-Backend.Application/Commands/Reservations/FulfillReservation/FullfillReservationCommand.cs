using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;

namespace Finnisimo_Library_Backend.Application.Commands.Reservations
    .FulfillReservation;

public sealed record FulfillReservationCommand(Guid BookId, Guid UserId)
    : ICommand<Guid>;
