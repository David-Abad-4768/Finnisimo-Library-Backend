using Finnisimo_Library_Backend.Domain.Abstractions;

namespace Finnisimo_Library_Backend.Domain.Entities.Books.Events;

public sealed record BookCreatedDomainEvent(Guid BookId) : IDomainEvent;
