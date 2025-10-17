using Finnisimo_Library_Backend.Domain.Abstractions;

namespace Finnisimo_Library_Backend.Domain.Entities.Reservations.Events;

public sealed record ReservationCancelledDomainEvent(Guid ReservationId,
                                                     Guid BookId)
    : IDomainEvent;
