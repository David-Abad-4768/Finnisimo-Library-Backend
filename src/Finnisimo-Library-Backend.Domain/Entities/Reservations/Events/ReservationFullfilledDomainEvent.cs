using Finnisimo_Library_Backend.Domain.Abstractions;

namespace Finnisimo_Library_Backend.Domain.Entities.Reservations.Events;

public sealed record ReservationFulfilledDomainEvent(Guid ReservationId,
                                                     Guid BookId, Guid UserId)
    : IDomainEvent;
