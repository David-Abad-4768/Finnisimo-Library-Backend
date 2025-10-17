using Finnisimo_Library_Backend.Domain.Abstractions;

namespace Finnisimo_Library_Backend.Domain.Entities.Reservations.Events;

public sealed record ReservationCreatedDomainEvent(Guid ReservationId,
                                                   Guid UserId, Guid BookId)
    : IDomainEvent;
