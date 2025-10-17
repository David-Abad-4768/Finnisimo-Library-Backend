using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;

namespace Finnisimo_Library_Backend.Application.Commands.Reservations
    .CancelReservation;

public sealed record CancelReservationCommand(Guid ReservationId, Guid UserId)
    : ICommand;
